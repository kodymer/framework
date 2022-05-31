using Vesta.Ddd.Application.Services;
using Vesta.Banks.Bank.Dtos;

namespace Vesta.Banks.Bank
{
    public interface IBankAppService : IApplicationService
    {
        Task CreateBankAccountAsync(CreateBankAccountInput input, CancellationToken cancellationToken = default);

        Task MakeTransferAsync(BankTransferInput input, CancellationToken cancellationToken = default);
    }
}