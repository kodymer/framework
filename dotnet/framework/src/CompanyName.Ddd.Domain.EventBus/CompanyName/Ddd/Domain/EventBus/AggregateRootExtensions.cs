using CommunityToolkit.Diagnostics;
using CompanyName.Ddd.Domain.Entities;
using System.Collections.Immutable;

namespace CompanyName.Ddd.Domain.EventBus
{
    public static class AggregateRootExtensions
    {
        public static void AddIntegrationEvent(this IIntegrationEventSource entity, object @event)
        {
            Guard.IsAssignableToType<IAggregateRoot>(entity);

            entity.As<IAggregateRoot>().IntegrationEvents.Add(new EventRecord(entity, @event, EventRecordOrderGenerator.GetNext()));
        }

        internal static ImmutableList<EventRecord> GetIntegrationEvents(this IIntegrationEventSource entity)
        {
            Guard.IsAssignableToType<IAggregateRoot>(entity);

            return entity.As<IAggregateRoot>().IntegrationEvents.ToImmutableList();
        }

        public static void ClearIntegrationEvents(this IIntegrationEventSource entity)
        {
            Guard.IsAssignableToType<IAggregateRoot>(entity);

            entity.As<IAggregateRoot>().IntegrationEvents.Clear();
        }

        public static void AddDomainEvent(this IDomainEventSource entity, object @event)
        {
            Guard.IsAssignableToType<IAggregateRoot>(entity);

            entity.As<IAggregateRoot>().DomainEvents.Add(new EventRecord(entity, @event, EventRecordOrderGenerator.GetNext()));
        }

        internal static ImmutableList<EventRecord> GetDomainEvents(this IDomainEventSource entity)
        {
            Guard.IsAssignableToType<IAggregateRoot>(entity);

            return entity.As<IAggregateRoot>().DomainEvents.ToImmutableList();
        }

        public static void ClearDomainEvents(this IDomainEventSource entity)
        {
            Guard.IsAssignableToType<IAggregateRoot>(entity);

            entity.As<IAggregateRoot>().DomainEvents.Clear();
        }
    }
}
