using System;
using Vesta.EntityFrameworkCore;
using Vesta.ProjectName.Bank;

namespace Vesta.ProjectName.EntityFrameworkCore.Repositories
{
    public class BankAccountRepository : ProjectNameRepositoryBase<BankAccount, Guid>, IBankAccountRepository
    {
        public BankAccountRepository(IDbContextProvider<ProjectNameDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
