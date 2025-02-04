using System.Threading;
using System.Threading.Tasks;
using CompanyName.Banks.Bank;

namespace CompanyName.Banks
{
    public interface IBankAccountManager
    {
        Task<BankAccount> CreateAsync(decimal initialBalance, CancellationToken cancellationToken = default);
    }
}