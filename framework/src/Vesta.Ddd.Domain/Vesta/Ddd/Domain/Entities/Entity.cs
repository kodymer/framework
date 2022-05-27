namespace Vesta.Ddd.Domain.Entities
{
    [Serializable]
    public abstract class Entity : IEntity
    {
        protected Entity()
        {

        }

        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}]";
        }

    }

    [Serializable]
    public abstract class Entity<TKey> : Entity, IEntity<TKey>
    {
        public virtual TKey Id { get; protected set; }

        protected Entity()
        {

        }

        protected Entity(TKey id)
        {
            Id = id;
        }

        public object[] GetKeys()
        {
            return new object[] { Id };
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}] Id = {Id}";
        }
    }

}
