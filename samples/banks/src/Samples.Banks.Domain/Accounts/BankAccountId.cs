using CompanyName.Ddd.Domain.Entities;

namespace Samples.Banks.Accounts
{
    public record class BankAccountId(Guid Value) : EntityId<Guid>(Value);
}