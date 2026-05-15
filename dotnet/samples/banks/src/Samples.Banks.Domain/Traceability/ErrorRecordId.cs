using CompanyName.Ddd.Domain.Entities;

namespace Samples.Banks.Traceability
{
    public record class ErrorRecordId(Guid Value) : EntityId<Guid>(Value);
}