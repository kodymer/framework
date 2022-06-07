using Vesta.Banks.Bank;
using Vesta.Ddd.Domain.Auditing;
using Vesta.EventBus;

namespace Vesta.Banks
{
    //[EventName("Vesta.Banks.Application.BankAccountDto")]
    public class BankAccount : FullAuditedAggregateRoot<Guid>
    {
        public const decimal MinimumOpeningAmount = 100.00m;
        public const int NameMaxLength = 80;


        public string Number { get; set; }
        public decimal Balance { get; private set; }

        public ICollection<BankTransfer> Debits { get; private set; }
        public ICollection<BankTransfer> Credits { get; private set; }

        public BankAccount(Guid id)
            : base(id)
        {
            Debits = new HashSet<BankTransfer>();
            Credits = new HashSet<BankTransfer>();
        }

        public void AssignOpeningBalance(decimal intialBalance)
        {
            if (intialBalance < MinimumOpeningAmount)
            {
                throw new UnfulfilledRequirementException("The account does not meet the minimum requirements to be opened.",
                    new InsufficientBalanceException("Insufficient opening balance"));
            }

            Balance = intialBalance;
        }

        public void Increase(decimal amount)
        {
            Balance += amount;
        }

        public void Decrease(decimal amount)
        {
            if ((Balance - amount) < decimal.Zero)
            {
                throw new InsufficientBalanceException("Insufficient balance");
            }

            Balance -= amount;
        }
    }
}
