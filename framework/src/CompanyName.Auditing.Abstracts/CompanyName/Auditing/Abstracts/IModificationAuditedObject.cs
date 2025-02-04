namespace CompanyName.Auditing.Abstracts
{
    public interface IModificationAuditedObject
    {
        DateTime? LastModificationTime { get; set; }

        Guid? LastModifierId { get; set; }
    }

}