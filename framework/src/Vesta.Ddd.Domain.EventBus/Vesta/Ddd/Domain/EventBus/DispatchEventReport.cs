namespace Vesta.Ddd.Domain.EventBus
{
    public class DispatchReport
    {
        public IEnumerable<EventRecord> IntegrationEvents { get; }

        public DispatchReport(IEnumerable<EventRecord> integrationEvents)
        {
            IntegrationEvents = integrationEvents;
        }
    }
}
