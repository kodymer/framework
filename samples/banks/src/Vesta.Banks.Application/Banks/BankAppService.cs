using Microsoft.Extensions.Logging;
using System.Text.Json;
using Vesta.Ddd.Application.Services;
using Vesta.Ddd.Domain.Entities;
using Vesta.Ddd.Domain.Repositories;
using Vesta.Banks.Bank.Dtos;
using Vesta.Banks.Domain;
using Vesta.Banks.Domain.Bank;
using Vesta.Banks.Application;

namespace Vesta.Banks.Bank
{
    public class BankAppService : ApplicationService, IBankAppService
    {
        private readonly IBankAccountRepository _repository;
        private readonly IBankAccountManager _bankAccountManager;
        private readonly IBankTransferService _bankTransferService;

        public BankAppService(
            IBankAccountRepository repository,
            IBankAccountManager bankAccountManager,
            IBankTransferService bankTransferService)
        {
            _repository = repository;
            _bankAccountManager = bankAccountManager;
            _bankTransferService = bankTransferService;
        }

        public async Task<List<BankAccountDto>> GetAllList()
        {
            var entities = await _repository.GetListAsync(orderBy: q => q.OrderBy(b => b.Number), includeProperties: new string[]
            {
                "Debits",
                "Credits"
            });

            return ObjectMapper.Map<List<BankAccountDto>>(entities);
        }

        public async Task CreateBankAccountAsync(CreateBankAccountInput input, CancellationToken cancellationToken = default)
        {
            try
            {
                Logger.LogInformation(BanksLogEventConsts.GenerateNewBankAccount,
                    "Creating a new bank account with balance {Balance}.", input.Balance);

                Logger.LogDebug(BanksLogEventConsts.GenerateNewBankAccount,
                    "Generating data for the new bank account. ");

                var bankAccount = await _bankAccountManager.CreateAsync(input.Balance);

                Logger.LogDebug(BanksLogEventConsts.GenerateNewBankAccount,
                    "Bank account: {Data}", JsonSerializer.Serialize(bankAccount));

                Logger.LogDebug(BanksLogEventConsts.GenerateNewBankAccount,
                    "Saving new bank account.", input.Balance);

                await _repository.InsertAsync(bankAccount, autoSave: true, cancellationToken);

                Logger.LogDebug(BanksLogEventConsts.GenerateNewBankAccount, "OK.", input.Balance);
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

        public async Task MakeTransferAsync(BankTransferInput input, CancellationToken cancellationToken = default)
        {
            try
            {
                Logger.LogInformation(BanksLogEventConsts.TransfersBetweenBankAccounts,
                    "Making a transfer beetween {FromId} and {ToId} bank accounts by €{Amount}", input.BankAccountFromId, input.BankAccountToId, input.Amount);


                Logger.LogDebug(BanksLogEventConsts.TransfersBetweenBankAccounts,
                    "Getting bank account from by ID: {Id}. ", input.BankAccountFromId);

                var bankAccountFrom = await _repository.GetAsync(input.BankAccountFromId);

                Logger.LogDebug(BanksLogEventConsts.TransfersBetweenBankAccounts,
                    "Getting bank account to by ID: {Id}. ", input.BankAccountToId);

                var bankAccountTo = await _repository.GetAsync(input.BankAccountToId);

                Logger.LogDebug(BanksLogEventConsts.TransfersBetweenBankAccounts,
                    "Making trasnfer by €{Id}. ", input.Amount);

                var bankTransfer = await _bankTransferService.MakeTransferAsync(bankAccountFrom, bankAccountTo, input.Amount);

                await _repository.UpdateAsync(bankAccountFrom, cancellationToken: cancellationToken);
                await _repository.UpdateAsync(bankAccountTo,  cancellationToken: cancellationToken);

                await CurrentUnitOfWork.SaveChangesAsync(cancellationToken);

                Logger.LogDebug(BanksLogEventConsts.TransfersBetweenBankAccounts,
                    "Successful transfer correctly. ", input.Amount);

            }
            catch (EntityNotFoundException e)
            {
                Logger.LogError(
                    BanksLogEventConsts.TransfersBetweenBankAccounts, e,
                    "Could not found entity");

                throw;
            }
            catch (Exception e)
            {
                Logger.LogError(
                   BanksLogEventConsts.TransfersBetweenBankAccounts, e,
                   "Unexpected error.");

                throw;
            }
        }
    }
}
