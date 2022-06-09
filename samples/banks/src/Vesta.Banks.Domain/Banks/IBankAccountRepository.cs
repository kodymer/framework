using System;
using Vesta.Ddd.Domain.Repositories;

namespace Vesta.Banks
{
    public interface IBankAccountRepository : IRepository<BankAccount, Guid>
    {
    }
}