using System;
using CompanyName.Ddd.Domain.Repositories;

namespace CompanyName.Banks
{
    public interface IBankAccountRepository : IRepository<BankAccount, Guid>
    {
    }
}