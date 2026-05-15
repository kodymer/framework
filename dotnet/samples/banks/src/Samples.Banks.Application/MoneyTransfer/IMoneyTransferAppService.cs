using CompanyName.Ddd.Application.Services;
using FluentResults;
using Samples.Banks.MoneyTransfers;

namespace Samples.Banks.MoneyTransfer
{
    public interface IMoneyTransferAppService : IApplicationService
    {

        Task<Result<List<BankTransferDto>>> GetTransferHistoryAsync(CancellationToken cancellationToken = default);

        Task<Result> MakeTransferAsync(CreateBankTransferCommand input, CancellationToken cancellationToken = default);
    }
}