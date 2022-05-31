using Microsoft.Extensions.Logging;
using System.Text.Json;
using Vesta.Ddd.Application.Services;
using Vesta.Ddd.Domain.Entities;
using Vesta.Ddd.Domain.Repositories;
using Vesta.ProjectName.Bank.Dtos;
using Vesta.ProjectName.Domain;
using Vesta.ProjectName.Domain.Bank;

namespace Vesta.ProjectName.Bank
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

        public async Task CreateBankAccountAsync(CreateBankAccountInput input, CancellationToken cancellationToken = default)
        {
            try
            {
                Logger.LogInformation(ProjectNameLogEventConsts.GenerateNewBankAccount,
                    "Creating a new bank account with balance {Balance}.", input.Balance);

                Logger.LogDebug(ProjectNameLogEventConsts.GenerateNewBankAccount,
                    "Generating data for the new bank account. ");

                var bankAccount = await _bankAccountManager.CreateAsync(input.Balance);

                Logger.LogDebug(ProjectNameLogEventConsts.GenerateNewBankAccount,
                    "Bank account: {Data}", JsonSerializer.Serialize(bankAccount));

                Logger.LogDebug(ProjectNameLogEventConsts.GenerateNewBankAccount,
                    "Saving new bank account.", input.Balance);

                await _repository.InsertAsync(bankAccount, autoSave: true, cancellationToken);

                Logger.LogDebug(ProjectNameLogEventConsts.GenerateNewBankAccount, "OK.", input.Balance);
            }
            catch (UnfulfilledRequirementException e)
            {
                Logger.LogError(
                    ProjectNameLogEventConsts.GenerateNewBankAccount, e,
                    "Could not create the bank account. See the exception detail for more details.");

                throw;
            }
            catch (Exception e)
            {
                Logger.LogError(
                   ProjectNameLogEventConsts.GenerateNewBankAccount, e,
                   "Unexpected error.");

                throw;
            }
        }

        public async Task MakeTransferAsync(BankTransferInput input, CancellationToken cancellationToken = default)
        {
            try
            {
                Logger.LogInformation(ProjectNameLogEventConsts.TransfersBetweenBankAccounts,
                    "Making a transfer beetween {FromId} and {ToId} bank accounts by €{Amount}", input.BankAccountFromId, input.BankAccountToId, input.Amount);


                Logger.LogDebug(ProjectNameLogEventConsts.TransfersBetweenBankAccounts,
                    "Getting bank account from by ID: {Id}. ", input.BankAccountFromId);

                var bankAccountFrom = await _repository.GetAsync(input.BankAccountFromId);

                Logger.LogDebug(ProjectNameLogEventConsts.TransfersBetweenBankAccounts,
                    "Getting bank account to by ID: {Id}. ", input.BankAccountToId);

                var bankAccountTo = await _repository.GetAsync(input.BankAccountToId);

                Logger.LogDebug(ProjectNameLogEventConsts.TransfersBetweenBankAccounts,
                    "Making trasnfer by €{Id}. ", input.Amount);

                _bankTransferService.MakeTransfer(bankAccountFrom, bankAccountTo, input.Amount);

                await _repository.UpdateAsync(bankAccountFrom, cancellationToken: cancellationToken) ;
                await _repository.UpdateAsync(bankAccountTo,  cancellationToken: cancellationToken);

                await CurrentUnitOfWork.SaveChangesAsync(cancellationToken);

                Logger.LogDebug(ProjectNameLogEventConsts.TransfersBetweenBankAccounts,
                    "Successful transfer correctly. ", input.Amount);

            }
            catch (EntityNotFoundException e)
            {
                Logger.LogError(
                    ProjectNameLogEventConsts.TransfersBetweenBankAccounts, e,
                    "Could not found entity");

                throw;
            }
            catch (Exception e)
            {
                Logger.LogError(
                   ProjectNameLogEventConsts.TransfersBetweenBankAccounts, e,
                   "Unexpected error.");

                throw;
            }
        }
    }
}
