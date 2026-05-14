using CompanyName.Ddd.Domain.Auditing;
using CompanyName.Ddd.Domain.Eventing;
using FluentResults;
using Samples.Banks.Transfers;

namespace Samples.Banks.Accounts
{
    [EventName("CompanyName.Banks.Etos.BankAccountChangedEto")]
    public class BankAccount : FullAuditedAggregateRoot<BankAccountId>
    {
        public const string TableName = "BankAccounts";
        public const int NameMaxLength = 80;
        public const decimal MinimumOpeningAmount = 100.00m;

        public bool IsActive { get; private set; }

        public string Number { get; set; }

        public decimal Balance { get; private set; }

        public BankAccount(BankAccountId id)
            : base(id)
        {
            IsActive = true;
        }

        public void AssignOpeningBalance(decimal intialBalance)
        {
            if (intialBalance < MinimumOpeningAmount)
            {
                throw new UnfulfilledRequirementException("The account does not meet the minimum requirements to be opened.",
                    new InsufficientBalanceException("Insufficient opening balance"));
            }

            Balance = intialBalance;

            AddDistributedEvent(new BankAccountCreatedEvent(Id.Value, Number, intialBalance));
        }

        public void Increase(decimal amount)
        {
            Balance += amount;

            AddDistributedEvent(this);
            AddLocalEvent(new BankAccountBalanceIncreasedEvent(Id.Value, Number, amount, Balance));
        }

        public Result Decrease(decimal amount)
        {
            if (Balance - amount < decimal.Zero)
            {
                return BankErrors.InsufficientBalance;
            }

            Balance -= amount;

            AddDistributedEvent(this);
            AddLocalEvent(new BankAccountBalanceDecreasedEvent(Id.Value, Number, amount, Balance));

            return Result.Ok();
        }

        public Result Activate()
        {
            var diff = DateTime.Now.Date - CreationTime.Date;
            if (diff.TotalDays > 180)
            {
                return BankErrors.CouldNotActivate;
            }

            return Result.Ok();
        }

        public Result Desactivate()
        {
            if (Balance >= 0)
            {
                return BankErrors.CouldNotDisactivate;
            }

            IsActive = false;

            return Result.Ok();
        }
    }
}