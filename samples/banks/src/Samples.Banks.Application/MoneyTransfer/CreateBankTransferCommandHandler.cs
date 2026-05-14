using CompanyName.Cqrs.Abstractions;
using CompanyName.Ddd.Domain.Entities;
using FluentResults;
using Microsoft.Extensions.Caching.Distributed;
using Samples.Banks.Accounts;
using Samples.Banks.Traceability;
using Samples.Banks.Transfers;

namespace Samples.Banks.MoneyTransfers
{
    public class CreateBankTransferCommandHandler : CommandHandler<CreateBankTransferCommand, Result<BankTransferDto>>
    {
        private readonly IBankTransferRepository _repository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IErrorRepository _errorRepository;
        private readonly IBankTransferService _bankTransferService;
        private readonly IDistributedCache _cache;

        public CreateBankTransferCommandHandler(
            IBankTransferRepository repository,
            IBankAccountRepository bankAccountRepository,
            IErrorRepository errorRepository,
            IBankTransferService bankTransferService,
            IDistributedCache cache)
        {
            _repository = repository;
            _bankAccountRepository = bankAccountRepository;
            _errorRepository = errorRepository;
            _bankTransferService = bankTransferService;
            _cache = cache;

            LocalizationResource = typeof(BanksResource);
        }

        public override async Task<Result<BankTransferDto>> HandleAsync(CreateBankTransferCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                Logger.MakingBankTransfersBetweenBankAccounts(command.BankAccountFromId, command.BankAccountToId, command.Amount);
                Logger.GettingBankAccountFrom(command.BankAccountFromId);

                var bankAccountFrom = await _bankAccountRepository.FirstOrDefaultAsync(new BankAccountByIdSpecification(command.BankAccountFromId), cancellationToken);

                Logger.GettingBankAccountTo(command.BankAccountToId);

                var bankAccountTo = await _bankAccountRepository.FirstOrDefaultAsync(new BankAccountByIdSpecification(command.BankAccountToId), cancellationToken);

                Logger.AddingTransferDetail(command.Amount);

                var bankTransfer = await _bankTransferService.MakeTransferAsync(bankAccountFrom, bankAccountTo, command.Amount);

                await _bankAccountRepository.UpdateAsync(bankAccountFrom, cancellationToken: cancellationToken);
                await _bankAccountRepository.UpdateAsync(bankAccountTo, cancellationToken: cancellationToken);

                await _repository.AddAsync(bankTransfer, cancellationToken);

                await CurrentUnitOfWork.CompleteAsync(cancellationToken); // Not affect Dapper Repository

                Logger.BankTransferCreated();

                // 
                // Publish event messages ad-hoc.
                // 
                // await _bankAccountPublisher.PublishAsync(bankAccountFrom, cancellationToken);
                // await _bankAccountPublisher.PublishAsync(bankAccountTo, cancellationToken);
                // 

                await _cache.RemoveAsync("GetAllBankAccountList", cancellationToken);

                var dto = ObjectMapper.Map<BankTransferDto>(bankTransfer);
                return Result.Ok(dto);
            }
            catch (EntityNotFoundException e)
            {
                Logger.EntityNotFound(e);

                Logger.CouldNotCreateBankTransfer(e);
                await CurrentUnitOfWork.RollbackAsync(cancellationToken);

                await _errorRepository.AddAsync(ErrorRecord.Create(e), true, cancellationToken: cancellationToken);

                return Result.Fail(new Error("Error caused by exception.").CausedBy(e));
            }
            catch (Exception e)
            {
                Logger.CouldNotCreateBankTransfer(e);
                await CurrentUnitOfWork.RollbackAsync(cancellationToken);

                await _errorRepository.AddAsync(ErrorRecord.Create(e), true, cancellationToken: cancellationToken);

                return Result.Fail(new Error("Error caused by exception.").CausedBy(e));
            }
        }
    }
}
