using CompanyName.Banks.Bank;
using CompanyName.Banks.EntityFrameworkCore;
using CompanyName.EntityFrameworkCore.Abstracts;

namespace CompanyName.Banks.EntityFrameworkCore.Repositories
{
    public class BankAccountRepository : BanksEfCoreRepositoryBase<BankAccount, Guid>, IBankAccountRepository
    {
        public BankAccountRepository(IDbContextProvider<BanksDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
