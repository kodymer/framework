using Ardalis.Specification;

namespace Samples.Banks.Accounts
{
    public class BankAccountsOrderedByNumberAscSpecification : Specification<BankAccount>
    {
        public BankAccountsOrderedByNumberAscSpecification(Guid branchId)
        {
            Query
                .OrderBy(bankAccount => bankAccount.Number)
                .AsNoTracking()
                .EnableCache($"{nameof(BankAccountsOrderedByNumberAscSpecification)}-{branchId}");
        }
    }
}
