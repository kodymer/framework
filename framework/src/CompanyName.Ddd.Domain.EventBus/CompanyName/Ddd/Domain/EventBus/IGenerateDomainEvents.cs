using System.Collections.Immutable;

namespace CompanyName.Ddd.Domain.EventBus
{
    internal interface IGenerateDomainEvents
    {
        internal ImmutableList<EventRecord> GetLocalEvents();

        void ClearLocalEvents();

        void AddLocalEvent(object @event);
    }


}
