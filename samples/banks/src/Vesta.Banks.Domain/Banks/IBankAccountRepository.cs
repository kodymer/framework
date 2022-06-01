using System;
using Vesta.Ddd.Domain.Repositories;

namespace Vesta.Banks.Bank
{
    public interface IBankAccountRepository : IRepository<BankAccount, Guid>
    {
    }
}