using Microsoft.EntityFrameworkCore.ChangeTracking;
using Vesta.Ddd.Domain.EventBus;
using Vesta.EventBus.Abstracts;

namespace Vesta.Uow
{
    public class UnitOfWorkEventRecordRegistrar : IUnitOfWorkEventRecordRegistrar
    {
        public IUnitOfWork UnitOfWork { get; }

        public UnitOfWorkEventRecordRegistrar(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public DispatchReport PrepareReport(IEnumerable<EntityEntry> entries)
        {
            var integrationEvents = new List<EventRecord>();

            var domainEvents = new List<EventRecord>();

            foreach (var entry in entries.ToList())
            {
                if (entry.Entity is IGenerateIntegrationEvents integrationEventGenerator)
                {
                    integrationEvents.AddRange(integrationEventGenerator.GetDistributedEvents());

                    integrationEventGenerator.ClearDistributedEvents();
                }

                if (entry.Entity is IGenerateDomainEvents domainEventGenerator)
                {
                    domainEvents.AddRange(domainEventGenerator.GetLocalEvents());

                    domainEventGenerator.ClearLocalEvents();
                }
            }

            return new DispatchReport(domainEvents, integrationEvents);
        }

        public async Task RegisterAsync(DispatchReport dispatchReport, CancellationToken cancellationToken = default)
        {
            foreach (var integrationEvent in dispatchReport.IntegrationEvents)
            {
                await UnitOfWork.AddEventRecordAsync<IDistributedEventBus>(
                    CreateUnitOfWorkEventRecord(integrationEvent.Source, integrationEvent.Data),
                    integrationEvent.Order, cancellationToken);
            }

            foreach (var domainEvents in dispatchReport.DomainEvents)
            {
                await UnitOfWork.AddEventRecordAsync<ILocalEventBus>(
                    CreateUnitOfWorkEventRecord(domainEvents.Source, domainEvents.Data), 
                    domainEvents.Order, cancellationToken);
            }
        }

        private UnitOfWorkEventRecord CreateUnitOfWorkEventRecord(object source, object data)
        {
            return new UnitOfWorkEventRecord(source, data);
        }
    }

    public class NullUnitOfWorkEventRecordRegistrar : IUnitOfWorkEventRecordRegistrar
    {
        private static IUnitOfWorkEventRecordRegistrar _instance;

        public static IUnitOfWorkEventRecordRegistrar Instance
        {
            get
            {
                _instance ??= new NullUnitOfWorkEventRecordRegistrar();
                return _instance;
            }
        }

        public DispatchReport PrepareReport(IEnumerable<EntityEntry> entries)
        {
            return new DispatchReport(new List<EventRecord>(), new List<EventRecord>());
        }

        public Task RegisterAsync(DispatchReport dispatchReport, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
