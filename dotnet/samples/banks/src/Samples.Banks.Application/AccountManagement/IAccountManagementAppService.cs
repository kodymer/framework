using CompanyName.Ddd.Application.Services;
using FluentResults;
using Samples.Banks.MoneyTransfers;

namespace Samples.Banks.AccountManagement
{
    public interface IAccountManagementAppService : IApplicationService
    {
        Task<Result> DoSomething(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result.Ok());
        }

        Task<Result<List<BankAccountDto>>> GetAllBankAccountListAsync(Guid branchId, CancellationToken cancellationToken = default);

        Task<Result> CreateBankAccountAsync(CreateBankAccountCommand input, CancellationToken cancellationToken = default);
    }
}