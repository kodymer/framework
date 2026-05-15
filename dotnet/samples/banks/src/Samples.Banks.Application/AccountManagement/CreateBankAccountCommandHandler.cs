using CompanyName.Cqrs.Abstractions;
using FluentResults;
using Samples.Banks.Accounts;
using Samples.Banks.Transfers;

namespace Samples.Banks.AccountManagement
{
    public class CreateBankAccountCommandHandler : CommandHandler<CreateBankAccountCommand, Result>
    {
        private readonly IBankAccountRepository _repository;
        private readonly IBankAccountManager _bankAccountManager;

        public CreateBankAccountCommandHandler(
            IBankAccountManager bankAccountManager,
            IBankAccountRepository repository)
        {
            _repository = repository;
            _bankAccountManager = bankAccountManager;

            LocalizationResource = typeof(BanksResource);
        }

        public override async Task<Result> HandleAsync(CreateBankAccountCommand command, CancellationToken cancellationToken = default)
        {

            try
            {
                Logger.GeneratingNewBankAccount(command.Balance);

                var bankAccount = await _bankAccountManager.CreateAsync(command.Balance);

                Logger.AddingBankAccountDetail(bankAccount);

                //  
                //  No send event message to Service Bus. It only save changes to the database.
                //  

                await _repository.AddAsync(bankAccount, true, cancellationToken);

                //  
                //  Send event messsage to Service Bus and save changes to the database.
                //  
                //  await _repository.AddAsync(bankAccount, cancellationToken: cancellationToken);
                //  
                //  await CurrentUnitOfWork.CompleteAsync(cancellationToken);
                //  

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
