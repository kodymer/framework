namespace CompanyName.Auditing.Abstractions
{
    public interface IModificationAuditedTimeObject
    {
        DateTime? LastModificationTime { get; set; }
    }

    public interface IModificationAuditedObject<TUserId> : IModificationAuditedTimeObject
        where TUserId : struct, IParsable<TUserId>
    {
        TUserId? LastModifierId { get; set; }
    }

    public interface IModificationAuditedObject : IModificationAuditedObject<Guid>
    {
    }

}