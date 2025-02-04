using CompanyName.Banks.Dtos;
using CompanyName.Ddd.Application.Services;

namespace CompanyName.Banks
{
    public interface IBankAppService : IApplicationService
    {

        Task<List<BankTransferOutput>> GetAllBankTransferListAsync(CancellationToken cancellationToken = default);

        Task<List<BankAccountDto>> GetAllBankAccountListAsync(CancellationToken cancellationToken = default);

        Task CreateBankAccountAsync(CreateBankAccountInput input, CancellationToken cancellationToken = default);

        Task MakeTransferAsync(BankTransferInput input, CancellationToken cancellationToken = default);
    }
}