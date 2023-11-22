using Autofac.Extras.DynamicProxy;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;
using Vesta.Banks.Domain;
using Vesta.Banks.Dtos;
using Vesta.Banks.Traceability;
using Vesta.Caching;
using Vesta.Ddd.Application.Services;
using Vesta.Ddd.Domain.Entities;
using Vesta.Uow;

namespace Vesta.Banks
{

    public class BankAppService : ApplicationService, IBankAppService
    {
        private readonly IDistributedCache _cache;
        private readonly IBankAccountRepository _repository;
        private readonly IBankTransferRepository _bankTransferRepository;
        private readonly IErrorRepository _errorRepository;
        private readonly IBankAccountManager _bankAccountManager;
        private readonly IBankTransferService _bankTransferService;
        private readonly IBankAccountPublisher _bankAccountPublisher;

        public BankAppService(
            IDistributedCache cache,
            IBankAccountRepository repository,
            IBankTransferRepository bankTransferRepository,
            IErrorRepository errorRepository,
            IBankAccountManager bankAccountManager,
            IBankTransferService bankTransferService,
            IBankAccountPublisher bankAccountPublisher)
        {
            _cache = cache;

            _repository = repository;
            _bankTransferRepository = bankTransferRepository;
            _errorRepository = errorRepository;
            _bankAccountManager = bankAccountManager;
            _bankTransferService = bankTransferService;
            _bankAccountPublisher = bankAccountPublisher;
        }

        public async Task<List<BankTransferOutput>> GetAllBankTransferListAsync(CancellationToken cancellationToken = default)
        {
            List<BankTransferOutput> dtos = null;

            try
            {
                Logger.LogInformation(BanksLogEventConsts.GetBankTransferHistory,
                    "Getting all bank transfers.");

                var entities = (await _bankTransferRepository.GelAllAsync(cancellationToken)).ToList();

                Logger.LogDebug(BanksLogEventConsts.GetBankTransferHistory,
                    "{Count} bank transfers have been obtained.", entities.Count());

                dtos = ObjectMapper.Map<List<BankTransferOutput>>(entities);
            }
            catch (Exception e)
            {

                Logger.LogError(BanksLogEventConsts.GetBankTransferHistory, e,
                    "Could not get the bank transfers. See the exception detail for more details.");

                throw;
            }

            return dtos;
        }

        public async Task<List<BankAccountDto>> GetAllBankAccountListAsync(CancellationToken cancellationToken = default)
        {
            List<BankAccountDto> dtos = null;

            try
            {
                Logger.LogInformation(BanksLogEventConsts.GetBankAccounts,
                "Getting all bank accounts.");

                var entities = await _cache.GetOrAddAsync(
                    "GetAllBankAccountList",
                    async () => await _repository.GetListAsync(orderBy: q => q.OrderBy(b => b.Number)));

                /*
                 * Without cache.
                 * 
                 * var entities = await _repository.GetListAsync(orderBy: q => q.OrderBy(b => b.Number));
                 */


                Logger.LogDebug(BanksLogEventConsts.GetBankAccounts,
                    "{Count} bank accounts have been obtained.", entities.Count);

                dtos = ObjectMapper.Map<List<BankAccountDto>>(entities);
            }
            catch (Exception e)
            {

                Logger.LogError(BanksLogEventConsts.GetBankAccounts, e,
                    "Could not get the bank accounts. See the exception detail for more details.");

                throw;
            }

            return dtos;
        }

        public async Task CreateBankAccountAsync(CreateBankAccountInput input, CancellationToken cancellationToken = default)
        {
            try
            {
                Logger.LogInformation(BanksLogEventConsts.GenerateNewBankAccount,
                    "Creating a new bank account with balance {Balance}.", input.Balance);

                var bankAccount = await _bankAccountManager.CreateAsync(input.Balance);

                Logger.LogDebug(BanksLogEventConsts.GenerateNewBankAccount,
                    "Bank account: {Data}", JsonSerializer.Serialize(bankAccount));


                /*
                 * No send event message to Service Bus. It only save changes to the database.
                 * 
                 */

                await _repository.InsertAsync(bankAccount, true, cancellationToken);

                /*
                 * Send event messsage to Service Bus and save changes to the database.
                 *
                 * await _repository.InsertAsync(bankAccount, cancellationToken: cancellationToken);
                 * 
                 * await CurrentUnitOfWork.CompleteAsync(cancellationToken);
                 * 
                 */


                Logger.LogInformation(BanksLogEventConsts.GenerateNewBankAccount,
                    "Bank account created!");
            }
            catch (UnfulfilledRequirementException e)
            {
                Logger.LogError(
                    BanksLogEventConsts.GenerateNewBankAccount, e,
                    "Could not create the bank account. See the exception detail for more details.");

                throw;
            }
            catch (Exception e)
            {
                Logger.LogError(
                   BanksLogEventConsts.GenerateNewBankAccount, e,
                   "Unexpected error.");

                throw;
            }
        }

        //[UnitOfWork(true, IsolationLevel.ReadCommitted, 3600)]
        public async Task MakeTransferAsync(BankTransferInput input, CancellationToken cancellationToken = default)
        {
            try
            {
                Logger.LogInformation(BanksLogEventConsts.TransfersBetweenBankAccounts,
                    "Making a transfer beetween {FromId} and {ToId} bank accounts by €{Amount}", input.BankAccountFromId, input.BankAccountToId, input.Amount);


                Logger.LogDebug(BanksLogEventConsts.TransfersBetweenBankAccounts,
                    "Getting bank account from by ID: {Id}. ", input.BankAccountFromId);

                var bankAccountFrom = await _repository.GetAsync(input.BankAccountFromId, cancellationToken);

                Logger.LogDebug(BanksLogEventConsts.TransfersBetweenBankAccounts,
                    "Getting bank account to by ID: {Id}. ", input.BankAccountToId);

                var bankAccountTo = await _repository.GetAsync(input.BankAccountToId, cancellationToken);

                Logger.LogDebug(BanksLogEventConsts.TransfersBetweenBankAccounts,
                    "Making trasnfer by €{Amount}. ", input.Amount);

                var bankTransfer = await _bankTransferService.MakeTransferAsync(bankAccountFrom, bankAccountTo, input.Amount);

                await _repository.UpdateAsync(bankAccountFrom, cancellationToken: cancellationToken);
                await _repository.UpdateAsync(bankAccountTo, cancellationToken: cancellationToken);

                await CurrentUnitOfWork.CompleteAsync(cancellationToken); // Not affect Dapper Repository

                Logger.LogInformation(BanksLogEventConsts.TransfersBetweenBankAccounts,
                    "Successful transfer correctly. ", input.Amount);

                /*
                 * await _bankTransferRepository.InsertAsync(bankTransfer, cancellationToken); // Dapper repository not support Unit of Work pattern
                 * 
                 * 
                 * Publish event messages.
                 * 
                 * await _bankAccountPublisher.PublishAsync(bankAccountFrom, cancellationToken);
                 * await _bankAccountPublisher.PublishAsync(bankAccountTo, cancellationToken);
                 * 
                 */

                //await _cache.RemoveAsync("GetAllBankAccountList", cancellationToken);

            }
            catch (EntityNotFoundException e)
            {
                Logger.LogError(
                    BanksLogEventConsts.TransfersBetweenBankAccounts, e,
                    "Could not found entity");

                await CurrentUnitOfWork.RollbackAsync(cancellationToken);

                await _errorRepository.InsertAsync(Error.Create(e), true, cancellationToken: cancellationToken);

                throw;
            }
            catch (Exception e)
            {
                Logger.LogError(
                   BanksLogEventConsts.TransfersBetweenBankAccounts, e,
                   "Unexpected error.");

                await CurrentUnitOfWork.RollbackAsync(cancellationToken);

                await _errorRepository.InsertAsync(Error.Create(e), true, cancellationToken: cancellationToken);

                throw;
            }
        }
    }
}
