using Microsoft.EntityFrameworkCore.ChangeTracking;
using Vesta.Ddd.Domain.EventBus;

namespace Vesta.Uow
{
    public interface IUnitOfWorkEventRecordRegistrar
    {
        DispatchReport PrepareReport(IEnumerable<EntityEntry> entries);

        Task RegisterAsync(DispatchReport dispatchReport, CancellationToken cancellationToken = default);
    }
}