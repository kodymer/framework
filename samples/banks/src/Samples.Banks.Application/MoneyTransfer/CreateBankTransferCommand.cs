using CompanyName.Cqrs.Abstractions;
using FluentResults;

namespace Samples.Banks.MoneyTransfers
{
    public record class CreateBankTransferCommand(
        Guid BankAccountFromId, 
        Guid BankAccountToId, 
        decimal Amount) :
        ICommand<Result<BankTransferDto>>;

}
