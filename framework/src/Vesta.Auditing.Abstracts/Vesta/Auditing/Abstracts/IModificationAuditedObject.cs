namespace Vesta.Auditing.Abstracts
{
    public interface IModificationAuditedObject
    {
        DateTime? LastModificationTime { get; set; }

        string LastModifierId { get; set; }
    }

}