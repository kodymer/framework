using Microsoft.EntityFrameworkCore.ChangeTracking;
using Vesta.Ddd.Domain.EventBus;

namespace Vesta.Uow
{
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
