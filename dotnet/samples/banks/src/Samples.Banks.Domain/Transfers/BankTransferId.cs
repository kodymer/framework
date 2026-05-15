using CompanyName.Ddd.Domain.Entities;

namespace Samples.Banks.Transfers
{
    public record class BankTransferId(long Value) : EntityId<long>(Value);
}