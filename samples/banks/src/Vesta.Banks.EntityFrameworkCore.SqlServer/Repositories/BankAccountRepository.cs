using System;
using Vesta.EntityFrameworkCore;
using Vesta.Banks.Bank;

namespace Vesta.Banks.EntityFrameworkCore.Repositories
{
    public class BankAccountRepository : BanksRepositoryBase<BankAccount, Guid>, IBankAccountRepository
    {
        public BankAccountRepository(IDbContextProvider<BanksDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
