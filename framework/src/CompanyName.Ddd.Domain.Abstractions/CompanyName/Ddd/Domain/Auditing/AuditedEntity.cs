using CompanyName.Auditing;

namespace CompanyName.Ddd.Domain.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="T:CompanyName.Domain.Auditing.IAuditedObject" /> for an entity.
    /// </summary>
    [Serializable]
    public abstract class AuditedEntity : CreationAuditedEntity, IAuditedObject
    {
        public virtual DateTime? LastModificationTime { get; set; }

        public virtual Guid? LastModifierId { get; set; }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAuditedObject"/> for an entity.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class AuditedEntity<TKey> : CreationAuditedEntity<TKey>, IAuditedObject
    {
        public virtual DateTime? LastModificationTime { get; set; }

        public virtual Guid? LastModifierId { get; set; }

        protected AuditedEntity()
        {

        }

        protected AuditedEntity(TKey id)
            : base(id)
        {

        }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAuditedObject{TUserId}"/> for an entity.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUserId">Type of the primary key of the user entity</typeparam>
    [Serializable]
    public abstract class AuditedEntity<TKey, TUserId> : CreationAuditedEntity<TKey, TUserId>, IAuditedObject<TUserId>
        where TUserId : struct, IParsable<TUserId>
    {
        public virtual DateTime? LastModificationTime { get; set; }

        public virtual TUserId? LastModifierId { get; set; }

        protected AuditedEntity()
        {

        }

        protected AuditedEntity(TKey id)
            : base(id)
        {

        }
    }
}
