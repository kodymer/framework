using Ardalis.Specification;

namespace Samples.Banks.Accounts
{
    public class BankAccountIsActiveSpecification : Specification<BankAccount>
    {
        public BankAccountIsActiveSpecification() : base()
        {

            Query
                .Where(bankAccount => bankAccount.IsActive);
        }
    }
}
