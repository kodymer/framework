namespace CompanyName.Auditing.Abstractions
{
    public interface IAuditedObject<TUserId> : ICreationAuditedObject<TUserId>, IModificationAuditedObject<TUserId>
        where TUserId : struct, IParsable<TUserId>
    {

    }

    public interface IAuditedObject 
        : IAuditedObject<Guid>
    {

    }
}