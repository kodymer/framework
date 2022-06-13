using Vesta.Auditing.Abstracts;

namespace Vesta.Ddd.Domain.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="T:Vesta.Domain.Auditing.IFullAuditedObject" /> for an entity.
    /// </summary>
    [Serializable]
    public abstract class FullAuditedAggregateRoot : AuditedAggregateRoot, IFullAuditedObject
    {
        public virtual bool IsDeleted { get; set; }

        public virtual DateTime? DeletionTime { get; set; }

        public virtual Guid? DeleterId { get; set; }

    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="IFullAuditedObject"/> for an entity.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class FullAuditedAggregateRoot<TKey> : AuditedAggregateRoot<TKey>, IFullAuditedObject
    {
        public virtual bool IsDeleted { get; set; }

        public virtual DateTime? DeletionTime { get; set; }

        public virtual Guid? DeleterId { get; set; }

        protected FullAuditedAggregateRoot()
        {

        }

        protected FullAuditedAggregateRoot(TKey id)
            : base(id)
        {

        }
    }
}
