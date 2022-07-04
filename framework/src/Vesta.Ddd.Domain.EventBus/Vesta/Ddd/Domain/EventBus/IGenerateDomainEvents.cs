using System.Collections.Immutable;

namespace Vesta.Ddd.Domain.EventBus
{
    internal interface IGenerateDomainEvents
    {
        internal ImmutableList<EventRecord> GetLocalEvents();

        void ClearLocalEvents();

        void AddLocalEvent(object @event);
    }


}
