using CompanyName.Ddd.Domain.Repositories;

namespace Samples.Banks.Accounts
{
    public interface IBankAccountRepository : IRepository<BankAccount>
    {
    }
}