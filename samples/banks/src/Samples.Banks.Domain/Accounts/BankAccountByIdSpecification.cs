using Ardalis.Specification;

namespace Samples.Banks.Accounts
{
    public class BankAccountByIdSpecification : Specification<BankAccount>
    {

        public BankAccountByIdSpecification(Guid id)
            : base()
        {
            Query
                .Where(bankAccount => bankAccount.Id == new BankAccountId(id));
        }
    }
}
