using System.Threading;
using System.Threading.Tasks;
using Vesta.Banks.Bank;

namespace Vesta.Banks
{
    public interface IBankAccountManager
    {
        Task<BankAccount> CreateAsync(decimal initialBalance, CancellationToken cancellationToken = default);
    }
}