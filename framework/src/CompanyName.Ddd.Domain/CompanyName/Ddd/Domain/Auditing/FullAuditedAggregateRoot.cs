using CompanyName.Auditing.Abstractions;

namespace CompanyName.Ddd.Domain.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="T:CompanyName.Domain.Auditing.IFullAuditedObject" /> for an entity.
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

    /// <summary>
    /// This class can be used to simplify implementing <see cref="IFullAuditedObject{TUserId}"/> for an entity.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUserId">Type of the primary key of the user entity</typeparam>
    [Serializable]
    public abstract class FullAuditedAggregateRoot<TKey, TUserId> : AuditedAggregateRoot<TKey, TUserId>, IFullAuditedObject<TUserId>
         where TUserId : struct, IParsable<TUserId>
    {
        public virtual bool IsDeleted { get; set; }

        public virtual DateTime? DeletionTime { get; set; }

        public virtual TUserId? DeleterId { get; set; }

        protected FullAuditedAggregateRoot()
        {

        }

        protected FullAuditedAggregateRoot(TKey id)
            : base(id)
        {

        }
    }
}
