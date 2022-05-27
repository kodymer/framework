namespace Vesta.Auditing.Abstracts
{
    public interface ICreationAuditedObject
    {
        DateTime CreationTime { get; set; }

        string CreatorId { get; set; }
    }
}