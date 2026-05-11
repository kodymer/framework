using CompanyName.Core;

namespace CompanyName.Auditing.Abstractions
{
    public interface IDeletionAuditedTimeObject
    {
        DateTime? DeletionTime { get; set; }
    }

    public interface IDeletionAuditedObject<TUserId> : ISoftDelete, IDeletionAuditedTimeObject
        where TUserId : struct, IParsable<TUserId>
    {
        TUserId? DeleterId { get; set; }
    }

    public interface IDeletionAuditedObject 
        : IDeletionAuditedObject<Guid>
    {
    }
}