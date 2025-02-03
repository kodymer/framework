using CompanyName.Auditing.Abstracts;

namespace CompanyName.Ddd.Domain.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="T:CompanyName.Domain.Auditing.IFullAuditedObject" /> for an entity.
    /// </summary>
    [Serializable]
    public abstract class FullAuditedEntity : AuditedEntity, IFullAuditedObject
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
    public abstract class FullAuditedEntity<TKey> : AuditedEntity<TKey>, IFullAuditedObject
    {
        public virtual bool IsDeleted { get; set; }

        public virtual DateTime? DeletionTime { get; set; }

        public virtual Guid? DeleterId { get; set; }

        protected FullAuditedEntity()
        {

        }

        protected FullAuditedEntity(TKey id)
            : base(id)
        {

        }
    }
}
