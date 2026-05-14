using Ardalis.Specification;
using Castle.Core.Resource;
using CompanyName.Ddd.Domain.Common.Filters;
using MassTransit.Internals.GraphValidation;

namespace Samples.Banks.Accounts
{
    public class BankAccountsOrderedByNumberAscSpecification : Specification<BankAccount>
    {
        public BankAccountsOrderedByNumberAscSpecification(Guid branchId)
        {
            Query
                .OrderBy(bankAccount => bankAccount.Number)
                .AsNoTracking()
                .EnableCache($"{ nameof(BankAccountsOrderedByNumberAscSpecification)}-{branchId}");
        }
    }
}
