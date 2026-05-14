using CompanyName.Cqrs.Abstractions;
using FluentResults;

namespace Samples.Banks.AccountManagement
{
    public record class CreateBankAccountCommand(
        decimal Balance) :
        ICommand<Result>;

}