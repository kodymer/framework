using CompanyName.Core;

namespace CompanyName.Auditing.Abstracts
{
    public interface IDeletionAuditedObject : ISoftDelete
    {
        DateTime? DeletionTime { get; set; }

        Guid? DeleterId { get; set; }
    }
}