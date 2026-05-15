using Samples.Banks.Accounts;

namespace Samples.Banks.EntityFrameworkCore.Repositories
{
    public class BankAccountRepository : Repository<BankAccount, Guid>, IBankAccountRepository
    {
        public BankAccountRepository(BankDbContext context)
            : base(context)
        {

        }
    }
}
