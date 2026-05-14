using CompanyName.Ddd.Application.Services;
using CompanyName.Ddd.Domain.Entities;
using FluentResults;
using Microsoft.Extensions.Caching.Distributed;
using Samples.Banks.Accounts;
using Samples.Banks.MoneyTransfers;
using Samples.Banks.Traceability;
using Samples.Banks.Transfers;

namespace Samples.Banks.MoneyTransfer
{
    public class MoneyTransferAppService : ApplicationService, IMoneyTransferAppService
    {
        private readonly IDistributedCache _cache;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IBankTransferDapperRepository _repository;
        private readonly IErrorRepository _errorRepository;
        private readonly IBankTransferService _bankTransferService;

        /*
         * private readonly IBankAccountPublisher _bankAccountPublisher;
         * 
         */

        public MoneyTransferAppService(
            IDistributedCache cache,
            IBankAccountRepository bankAccountRepository,
            IBankTransferDapperRepository repository,
            IErrorRepository errorRepository,
            IBankTransferService bankTransferService
              /*
               * , 
               * IBankAccountPublisher bankAccountPublisher
               */)
        {
            _cache = cache;
            _bankAccountRepository = bankAccountRepository;
            _repository = repository;
            _errorRepository = errorRepository;
            _bankTransferService = bankTransferService;

            LocalizationResource = typeof(BanksResource);

            /*
             *  _bankAccountPublisher = bankAccountPublisher
             */
        }

        public async Task<Result<List<BankTransferDto>>> GetTransferHistoryAsync(CancellationToken cancellationToken = default)
        {
            List<BankTransferDto> dtos = null;

            try
            {
                Logger.GettingAllBankTransferHistory();

                var entities = (await _repository.ListAsync(cancellationToken)).ToList();

                Logger.BankTransferHistoryCountObtained(entities.Count);

                dtos = ObjectMapper.Map<List<BankTransferDto>>(entities);
                return Result.Ok(dtos);
            }
            catch (Exception e)
            {
                Logger.CouldNotGetBankTransfers(e);

                return Result.Fail(new Error("Error caused by exception.").CausedBy(e));
            }
        }

        //[UnitOfWork(true, IsolationLevel.ReadCommitted, 3600)]
        public async Task<Result> MakeTransferAsync(CreateBankTransferCommand input, CancellationToken cancellationToken = default)
        {
            try
            {
                Logger.MakingBankTransfersBetweenBankAccounts(input.BankAccountFromId, input.BankAccountToId, input.Amount);

                Logger.GettingBankAccountFrom(input.BankAccountFromId);

                var bankAccountFrom = await _bankAccountRepository.FirstOrDefaultAsync(new BankAccountByIdSpecification(input.BankAccountFromId), cancellationToken);

                Logger.GettingBankAccountTo(input.BankAccountToId);

                var bankAccountTo = await _bankAccountRepository.FirstOrDefaultAsync(new BankAccountByIdSpecification(input.BankAccountToId), cancellationToken);

                Logger.AddingTransferDetail(input.Amount);

                var bankTransfer = await _bankTransferService.MakeTransferAsync(bankAccountFrom, bankAccountTo, input.Amount);

                await _bankAccountRepository.UpdateAsync(bankAccountFrom, cancellationToken: cancellationToken);
                await _bankAccountRepository.UpdateAsync(bankAccountTo, cancellationToken: cancellationToken);

                await CurrentUnitOfWork.CompleteAsync(cancellationToken); // Not affect Dapper Repository

                Logger.BankTransferCreated();

                /*
                 * await _bankTransferRepository.AddAsync(bankTransfer, cancellationToken); // Dapper repository not support Unit of Work pattern
                 * 
                 * 
                 * Publish event messages.
                 * 
                 * await _bankAccountPublisher.PublishAsync(bankAccountFrom, cancellationToken);
                 * await _bankAccountPublisher.PublishAsync(bankAccountTo, cancellationToken);
                 * 
                 */

                //await _cache.RemoveAsync("GetAllBankAccountList", cancellationToken);

                return Result.Ok();
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
