using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public DispatchReport Prepare(IEnumerable<EntityEntry> entries)
        {
            var integrationEventRecords = new List<EventRecord>();

            foreach (var entry in entries.ToList())
            {
                if (entry.Entity is IGenerateIntegrationEvents entity)
                {
                    integrationEventRecords.AddRange(entity.GetDistributedEvents());

                    entity.ClearDistributedEvents();
                }
            }

            return new DispatchReport(integrationEventRecords);
        }

        public async Task RegisterAsync(DispatchReport dispatchReport, CancellationToken cancellationToken = default)
        {
            foreach (var integrationEvent in dispatchReport.IntegrationEvents)
            {
                var unitOfWorkEventRecord = new UnitOfWorkEventRecord(integrationEvent.Source, integrationEvent.Data);
                await UnitOfWork.AddEventRecordAsync<IDistributedEventBus>(
                    unitOfWorkEventRecord, integrationEvent.Order, cancellationToken);
            }
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

        public DispatchReport Prepare(IEnumerable<EntityEntry> entries)
        {
            return new DispatchReport(new List<EventRecord>());
        }

        public Task RegisterAsync(DispatchReport dispatchReport, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
