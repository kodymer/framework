using CompanyName.Ddd.Domain.EventBus;

namespace CompanyName.Ddd.Domain.Entities
{
    public interface IAggregateRoot : IEntity
    {

        internal ICollection<EventRecord> DomainEvents { get; }

        internal ICollection<EventRecord> IntegrationEvents { get; }

    }
}