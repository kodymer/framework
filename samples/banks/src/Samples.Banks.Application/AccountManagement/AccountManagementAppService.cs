using CompanyName.Caching;
using CompanyName.Ddd.Application.Services;
using FluentResults;
using Microsoft.Extensions.Caching.Distributed;
using Samples.Banks.Accounts;
using Samples.Banks.Transfers;

namespace Samples.Banks.AccountManagement
{
    public class AccountManagementAppService : ApplicationService, IAccountManagementAppService
    {
        private readonly IDistributedCache _cache;
        private readonly IBankAccountRepository _repository;
        private readonly IBankAccountManager _bankAccountManager;

        /*
         * private readonly IBankAccountPublisher _bankAccountPublisher;
         * 
         */

        public AccountManagementAppService(
            IDistributedCache cache,
            IBankAccountRepository repository,
            IBankAccountManager bankAccountManager,
            IBankAccountPublisher bankAccountPublisher)
        {
            _cache = cache;
            _repository = repository;
            _bankAccountManager = bankAccountManager;

            LocalizationResource = typeof(BanksResource);

            /*
             * _bankAccountPublisher = bankAccountPublisher;
             * 
             */
        }

        public async Task<Result<List<BankAccountDto>>> GetAllBankAccountListAsync(Guid branchId, CancellationToken cancellationToken = default)
        {
            List<BankAccountDto> dtos = null;

            try
            {
                Logger.GettingAllBankAccounts();

                var entities = await _cache.GetOrAddAsync(
                    "GetAllBankAccountList",
                    async () => await _repository.ListAsync(new BankAccountsOrderedByNumberAscSpecification(branchId), cancellationToken));

                Logger.BankAccountCountObtained(entities.Count);

                /*
                 * Without cache.
                 * 
                 * var entities = await _repository.ListAsync(new BankAccountsOrderedByNumberAsc(), cancellationToken));
                 */

                dtos = ObjectMapper.Map<List<BankAccountDto>>(entities);
                return Result.Ok(dtos);
            }
            catch (Exception e)
            {
                Logger.CouldNotGetBankAccounts(e);

                return Result.Fail(new Error("Error caused by exception.").CausedBy(e));
            }

        }

        public async Task<Result> CreateBankAccountAsync(CreateBankAccountCommand input, CancellationToken cancellationToken = default)
        {
            try
            {
                Logger.GeneratingNewBankAccount(input.Balance);

                var bankAccount = await _bankAccountManager.CreateAsync(input.Balance);

                Logger.AddingBankAccountDetail(bankAccount);

                /*
                 * No send event message to Service Bus. It only save changes to the database.
                 * 
                 */

                await _repository.AddAsync(bankAccount, cancellationToken);

                /*
                 * Send event messsage to Service Bus and save changes to the database.
                 *
                 * await _repository.AddAsync(bankAccount, false, cancellationToken: cancellationToken);
                 * 
                 * await CurrentUnitOfWork.CompleteAsync(cancellationToken);
                 * 
                 */

                Logger.BankAccountCreated();

                return Result.Ok();
            }
            catch (UnfulfilledRequirementException e)
            {
                Logger.CouldNotCreateBankAccount(e);

                return Result.Fail(new Error("Error caused by exception.").CausedBy(e));
            }
            catch (Exception e)
            {
                Logger.CouldNotCreateBankAccount(e);

                return Result.Fail(new Error("Error caused by exception.").CausedBy(e));
            }
        }
    }
}
