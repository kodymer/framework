namespace Vesta.Auditing.Abstracts
{
    public interface ICreationAuditedObject
    {
        DateTime CreationTime { get; set; }

        Guid? CreatorId { get; set; }
    }
}