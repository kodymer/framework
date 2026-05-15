using CompanyName.Auditing;
using CompanyName.Ddd.Domain.Entities;

namespace CompanyName.Ddd.Domain.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="T:Volo.Abp.Auditing.ICreationAuditedObject" /> for an entity.
    /// </summary>
    [Serializable]
    public abstract class CreationAuditedEntity : Entity, ICreationAuditedObject
    {
        public virtual DateTime CreationTime { get; set; }

        public virtual Guid? CreatorId { get; set; }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAuditedObject"/> for an entity.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class CreationAuditedEntity<TKey> : Entity<TKey>, ICreationAuditedObject
    {
        public virtual DateTime CreationTime { get; set; }

        public virtual Guid? CreatorId { get; set; }

        protected CreationAuditedEntity()
        {

        }

        protected CreationAuditedEntity(TKey id)
            : base(id)
        {

        }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAuditedObject{TUserId}"/> for an entity.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUserId">Type of the primary key of the user entity</typeparam>
    [Serializable]
    public abstract class CreationAuditedEntity<TKey, TUserId> : Entity<TKey>, ICreationAuditedObject<TUserId>
         where TUserId : struct, IParsable<TUserId>
    {
        public virtual DateTime CreationTime { get; set; }

        public virtual TUserId? CreatorId { get; set; }

        protected CreationAuditedEntity()
        {

        }

        protected CreationAuditedEntity(TKey id)
            : base(id)
        {

        }
    }
}
