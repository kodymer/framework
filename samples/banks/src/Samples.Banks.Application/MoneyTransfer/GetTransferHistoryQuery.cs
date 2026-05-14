using CompanyName.Cqrs.Abstractions;
using FluentResults;

namespace Samples.Banks.MoneyTransfers
{
    public record class GetTransferHistoryQuery : 
        IQuery<Result<List<BankTransferDto>>>;
}
