namespace CompanyName.Auditing.Abstractions
{
    public interface IFullAuditedObject<TUserId> : IAuditedObject<TUserId>, IDeletionAuditedObject<TUserId>
        where TUserId : struct, IParsable<TUserId>
    {

    }

    public interface IFullAuditedObject 
        : IFullAuditedObject<Guid>, IAuditedObject, IDeletionAuditedObject
    {

    }
}