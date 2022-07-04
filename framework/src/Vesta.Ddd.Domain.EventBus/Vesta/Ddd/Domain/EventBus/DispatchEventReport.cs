namespace Vesta.Ddd.Domain.EventBus
{
    public class DispatchReport
    {
        public IEnumerable<EventRecord> DomainEvents { get; }

        public IEnumerable<EventRecord> IntegrationEvents { get; }


        public DispatchReport(
            IEnumerable<EventRecord> domainEvents,
            IEnumerable<EventRecord> integrationEvents)
        {
            DomainEvents = domainEvents;
            IntegrationEvents = integrationEvents;
        }
    }
}
