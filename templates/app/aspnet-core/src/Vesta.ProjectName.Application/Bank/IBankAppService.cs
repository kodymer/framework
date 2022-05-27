using Vesta.Ddd.Application.Services;
using Vesta.ProjectName.Bank.Dtos;

namespace Vesta.ProjectName.Bank
{
    public interface IBankAppService : IApplicationService
    {
        Task CreateBankAccountAsync(CreateBankAccountInput input, CancellationToken cancellationToken = default);

        Task MakeTransferAsync(BankTransferInput input, CancellationToken cancellationToken = default);
    }
}