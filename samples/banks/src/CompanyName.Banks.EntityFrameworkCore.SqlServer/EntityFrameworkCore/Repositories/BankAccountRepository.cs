using CompanyName.Banks.Bank;
using CompanyName.Banks.EntityFrameworkCore;
using CompanyName.EntityFrameworkCore.Abstractions;

namespace CompanyName.Banks.EntityFrameworkCore.Repositories
{
    public class BankAccountRepository : Repository<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(BanksDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
