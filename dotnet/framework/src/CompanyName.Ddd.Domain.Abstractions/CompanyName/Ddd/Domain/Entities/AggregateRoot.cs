using CompanyName.Ddd.Domain.EventBus;
using System.Collections.ObjectModel;

namespace CompanyName.Ddd.Domain.Entities
{

    [Serializable]
    public abstract class AggregateRoot : Entity,
        IHasConcurrencyStamp, IAggregateRoot
    {

        public virtual string ConcurrencyStamp { get; set; }

        ICollection<EventRecord> IAggregateRoot.DomainEvents { get; } = new Collection<EventRecord>();

        ICollection<EventRecord> IAggregateRoot.IntegrationEvents { get; } = new Collection<EventRecord>();

        protected AggregateRoot()
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }
    }

    [Serializable]
    public abstract class AggregateRoot<TKey> : Entity<TKey>,
        IHasConcurrencyStamp, IAggregateRoot
    {

        public virtual string ConcurrencyStamp { get; set; }

        ICollection<EventRecord> IAggregateRoot.DomainEvents { get; } = new Collection<EventRecord>();

        ICollection<EventRecord> IAggregateRoot.IntegrationEvents { get; } = new Collection<EventRecord>();

        protected AggregateRoot()
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        protected AggregateRoot(TKey id)
            : this()

        {
            Id = id;
        }
    }
}

