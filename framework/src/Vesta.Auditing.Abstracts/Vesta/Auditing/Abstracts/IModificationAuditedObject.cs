namespace Vesta.Auditing.Abstracts
{
    public interface IModificationAuditedObject
    {
        DateTime? LastModificationTime { get; set; }

        Guid? LastModifierId { get; set; }
    }

}