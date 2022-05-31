using System.Threading;
using System.Threading.Tasks;
using Vesta.Banks.Bank;

namespace Vesta.Banks.Domain.Bank
{
    public interface IBankAccountManager
    {
        Task<BankAccount> CreateAsync(decimal initialBalance, CancellationToken cancellationToken = default);
    }
}