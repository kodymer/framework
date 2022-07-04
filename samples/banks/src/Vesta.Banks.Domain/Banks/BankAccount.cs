using Vesta.Banks.Bank;
using Vesta.Banks.Etos;
using Vesta.Ddd.Domain.Auditing;
using Vesta.Ddd.Domain.EventBus;

namespace Vesta.Banks
{
    [EventName("Vesta.Banks.Etos.BankAccountChangedEto")]
    public class BankAccount : FullAuditedAggregateRoot<Guid>
    {
        public const string TableName = "BankAccounts";
        public const int NameMaxLength = 80;
        public const decimal MinimumOpeningAmount = 100.00m;


        public string Number { get; set; }
        public decimal Balance { get; private set; }

        public BankAccount(Guid id)
            : base(id)
        {

        }

        public void AssignOpeningBalance(decimal intialBalance)
        {
            if (intialBalance < MinimumOpeningAmount)
            {
                throw new UnfulfilledRequirementException("The account does not meet the minimum requirements to be opened.",
                    new InsufficientBalanceException("Insufficient opening balance"));
            }

            Balance = intialBalance;

            AddDistributedEvent(new BankAccountCreatedEto()
            {
                Id = Id,
                Number = Number,
                InitialBalance = intialBalance,
            });
        }

        public void Increase(decimal amount)
        {
            Balance += amount;

            AddDistributedEvent(this);
            AddLocalEvent(new BankAccountBalanceIncreasedEto()
            {
                Id = Id,
                Number = Number,
                AddedAmount = amount,
                Balance = Balance
            });
        }

        public void Decrease(decimal amount)
        {
            if ((Balance - amount) < decimal.Zero)
            {
                throw new InsufficientBalanceException("Insufficient balance");
            }

            Balance -= amount;

            AddDistributedEvent(this);
            AddLocalEvent(new BankAccountBalanceDecreasedEto()
            {
                Id = Id,
                Number = Number,
                SubtractedAmount = amount,
                Balance = Balance
            });
        }
    }
}