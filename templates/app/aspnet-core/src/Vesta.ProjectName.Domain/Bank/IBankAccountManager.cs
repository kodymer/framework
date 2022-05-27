using System.Threading;
using System.Threading.Tasks;
using Vesta.ProjectName.Bank;

namespace Vesta.ProjectName.Domain.Bank
{
    public interface IBankAccountManager
    {
        Task<BankAccount> CreateAsync(decimal initialBalance, CancellationToken cancellationToken = default);
    }
}