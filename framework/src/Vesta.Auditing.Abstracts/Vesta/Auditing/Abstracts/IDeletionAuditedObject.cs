using Vesta.Core;

namespace Vesta.Auditing.Abstracts
{
    public interface IDeletionAuditedObject : ISoftDelete
    {
        DateTime? DeletionTime { get; set; }

        string DeleterId { get; set; }
    }
}