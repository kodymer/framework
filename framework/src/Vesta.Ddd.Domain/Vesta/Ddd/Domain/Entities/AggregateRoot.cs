using Vesta.Ddd.Domain.EventBus;

namespace Vesta.Ddd.Domain.Entities
{
    [Serializable]
    public abstract class AggregateRoot : Entity, 
        IHasConcurrencyStamp, IDomainEvents
    {

        public virtual string ConcurrencyStamp { get; set; }

        protected AggregateRoot()
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

    }


    [Serializable]
    public abstract class AggregateRoot<TKey> : Entity<TKey>, 
        IHasConcurrencyStamp, IDomainEvents
    {

        public virtual string ConcurrencyStamp { get; set; }

        protected AggregateRoot()
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        protected AggregateRoot(TKey id)
        {
            Id = id;
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }
    }
}

