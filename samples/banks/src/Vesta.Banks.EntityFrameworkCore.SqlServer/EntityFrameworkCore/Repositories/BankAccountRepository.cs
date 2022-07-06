using Vesta.Banks.Bank;
using Vesta.Banks.EntityFrameworkCore;
using Vesta.EntityFrameworkCore.Abstracts;

namespace Vesta.Banks.EntityFrameworkCore.Repositories
{
    public class BankAccountRepository : BanksEfCoreRepositoryBase<BankAccount, Guid>, IBankAccountRepository
    {
        public BankAccountRepository(IDbContextProvider<BanksDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
