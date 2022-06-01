using Vesta.Ddd.Application.Services;
using Vesta.Banks.Bank.Dtos;
using Vesta.Banks.Application;

namespace Vesta.Banks.Bank
{
    public interface IBankAppService : IApplicationService
    {
        Task<List<BankAccountDto>> GetAllList();

        Task CreateBankAccountAsync(CreateBankAccountInput input, CancellationToken cancellationToken = default);

        Task MakeTransferAsync(BankTransferInput input, CancellationToken cancellationToken = default);
    }
}