namespace CompanyName.Auditing.Abstractions
{
    public interface ICreationAuditedTimeObject
    {
        DateTime CreationTime { get; set; }
    }

    public interface ICreationAuditedObject<TUserId> : ICreationAuditedTimeObject
        where TUserId : struct, IParsable<TUserId>
    {

        TUserId? CreatorId { get; set; }
    }

    public interface ICreationAuditedObject
        : ICreationAuditedObject<Guid>
    {

    }
}