using Vesta.Auditing.Abstracts;
using Vesta.Ddd.Domain.Entities;

namespace Vesta.Ddd.Domain.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="T:Volo.Abp.Auditing.ICreationAuditedObject" /> for an entity.
    /// </summary>
    [Serializable]
    public abstract class CreationAuditedAggregateRoot : AggregateRoot, ICreationAuditedObject
    {
        public virtual DateTime CreationTime { get; set; }

        public virtual Guid? CreatorId { get; set; }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAuditedObject"/> for an entity.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class CreationAuditedAggregateRoot<TKey> : AggregateRoot<TKey>, ICreationAuditedObject
    {
        public virtual DateTime CreationTime { get; set; }

        public virtual Guid? CreatorId { get; set; }

        protected CreationAuditedAggregateRoot()
        {

        }

        protected CreationAuditedAggregateRoot(TKey id)
            : base(id)
        {

        }
    }
}
