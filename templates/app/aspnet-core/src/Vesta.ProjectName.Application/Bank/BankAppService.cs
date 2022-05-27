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
        private readonly ILogger<BankAppService> _logger;

        private readonly IBankAccountRepository _repository;
        private readonly IBankAccountManager _bankAccountManager;
        private readonly IBankTransferService _bankTransferService;

        public BankAppService(
            ILogger<BankAppService> logger,
            IBankAccountRepository repository,
            IBankAccountManager bankAccountManager,
            IBankTransferService bankTransferService)
        {
            _logger = logger;

            _repository = repository;
            _bankAccountManager = bankAccountManager;
            _bankTransferService = bankTransferService;
        }

        public async Task CreateBankAccountAsync(CreateBankAccountInput input, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation(ProjectNameLogEventConsts.GenerateNewBankAccount,
                    "Creating a new bank account with balance {Balance}.", input.Balance);

                _logger.LogDebug(ProjectNameLogEventConsts.GenerateNewBankAccount,
                    "Generating data for the new bank account. ");

                var bankAccount = await _bankAccountManager.CreateAsync(input.Balance);

                _logger.LogDebug(ProjectNameLogEventConsts.GenerateNewBankAccount,
                    "Bank account: {Data}", JsonSerializer.Serialize(bankAccount));

                _logger.LogDebug(ProjectNameLogEventConsts.GenerateNewBankAccount,
                    "Saving new bank account.", input.Balance);

                await _repository.InsertAsync(bankAccount, autoSave: true, cancellationToken);

                _logger.LogDebug(ProjectNameLogEventConsts.GenerateNewBankAccount, "OK.", input.Balance);
            }
            catch (UnfulfilledRequirementException e)
            {
                _logger.LogError(
                    ProjectNameLogEventConsts.GenerateNewBankAccount, e,
                    "Could not create the bank account. See the exception detail for more details.");

                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(
                   ProjectNameLogEventConsts.GenerateNewBankAccount, e,
                   "Unexpected error.");

                throw;
            }
        }

        public async Task MakeTransferAsync(BankTransferInput input, CancellationToken cancellationToken = default)
        {
            try
            {
                var bankAccountFrom = await _repository.GetAsync(input.BankAccountFromId);
                var bankAccountTo = await _repository.GetAsync(input.BankAccountToId);

                _bankTransferService.MakeTransfer(bankAccountFrom, bankAccountTo, input.Amount);

                await _repository.UpdateAsync(bankAccountFrom, cancellationToken: cancellationToken) ;
                await _repository.UpdateAsync(bankAccountTo,  cancellationToken: cancellationToken);

                await CurrentUnitOfWork.SaveChangesAsync(cancellationToken);

            }
            catch (EntityNotFoundException)
            {
                // Logging error

                throw;
            }
            catch (Exception)
            {
                // Logging error

                throw;
            }
        }
    }
}
