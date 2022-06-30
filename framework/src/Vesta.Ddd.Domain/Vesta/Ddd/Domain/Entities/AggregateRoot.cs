using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Vesta.Ddd.Domain.EventBus;

namespace Vesta.Ddd.Domain.Entities
{


    [Serializable]
    public abstract class AggregateRoot : Entity,
        IHasConcurrencyStamp, IGenerateIntegrationEvents
    {

        public virtual string ConcurrencyStamp { get; set; }

        private ICollection<EventRecord> _distributedEvents;

        protected AggregateRoot()
        {
            _distributedEvents = new Collection<EventRecord>();

            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        public virtual ImmutableList<EventRecord> GetDistributedEvents()
        {
            return _distributedEvents.ToImmutableList();
        }

        public virtual void ClearDistributedEvents()
        {
            _distributedEvents.Clear();
        }

        public virtual void AddDistributedEvent(object @event)
        {
            _distributedEvents.Add(new EventRecord(this, @event, EventRecordOrderGenerator.GetNext()));
        }
    }

    [Serializable]
    public abstract class AggregateRoot<TKey> : Entity<TKey>,
        IHasConcurrencyStamp, IGenerateIntegrationEvents
    {

        public virtual string ConcurrencyStamp { get; set; }

        private ICollection<EventRecord> _distributedEvents;

        protected AggregateRoot()
        {
            _distributedEvents = new Collection<EventRecord>();

            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        protected AggregateRoot(TKey id)
            : base()

        {
            Id = id;
        }

        ImmutableList<EventRecord> IGenerateIntegrationEvents.GetDistributedEvents()
        {
            return _distributedEvents.ToImmutableList();
        }

        public virtual void ClearDistributedEvents()
        {
            _distributedEvents.Clear();
        }

        public virtual void AddDistributedEvent(object @event)
        {
            _distributedEvents.Add(new EventRecord(this, @event, EventRecordOrderGenerator.GetNext()));
        }
    }
}

