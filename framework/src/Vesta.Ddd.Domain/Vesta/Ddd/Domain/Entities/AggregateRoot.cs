using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Vesta.Ddd.Domain.EventBus;

namespace Vesta.Ddd.Domain.Entities
{


    [Serializable]
    public abstract class AggregateRoot : Entity,
        IHasConcurrencyStamp, IGenerateIntegrationEvents, IGenerateDomainEvents
    {

        public virtual string ConcurrencyStamp { get; set; }

        private ICollection<EventRecord> _localEvents;
        private ICollection<EventRecord> _distributedEvents;

        protected AggregateRoot()
        {
            _localEvents = new Collection<EventRecord>();
            _distributedEvents = new Collection<EventRecord>();

            ConcurrencyStamp = Guid.NewGuid().ToString("N");
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

        ImmutableList<EventRecord> IGenerateDomainEvents.GetLocalEvents()
        {
            return _localEvents.ToImmutableList();
        }

        public virtual void ClearLocalEvents()
        {
            _localEvents.Clear();
        }
        public virtual void AddLocalEvent(object @event)
        {
            _localEvents.Add(new EventRecord(this, @event, EventRecordOrderGenerator.GetNext()));
        }

    }

    [Serializable]
    public abstract class AggregateRoot<TKey> : Entity<TKey>,
        IHasConcurrencyStamp, IGenerateIntegrationEvents, IGenerateDomainEvents
    {

        public virtual string ConcurrencyStamp { get; set; }

        private ICollection<EventRecord> _localEvents;
        private ICollection<EventRecord> _distributedEvents;

        protected AggregateRoot()
            : base()
        {
            _localEvents = new Collection<EventRecord>();
            _distributedEvents = new Collection<EventRecord>();

            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        protected AggregateRoot(TKey id)
            : this()

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

        ImmutableList<EventRecord> IGenerateDomainEvents.GetLocalEvents()
        {
            return _localEvents.ToImmutableList();
        }

        public virtual void ClearLocalEvents()
        {
            _localEvents.Clear();
        }
        public virtual void AddLocalEvent(object @event)
        {
            _localEvents.Add(new EventRecord(this, @event, EventRecordOrderGenerator.GetNext()));
        }
    }
}

