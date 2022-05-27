using System;
using Vesta.Ddd.Domain.Repositories;

namespace Vesta.ProjectName.Bank
{
    public interface IBankAccountRepository : IRepository<BankAccount, Guid>
    {
    }
}