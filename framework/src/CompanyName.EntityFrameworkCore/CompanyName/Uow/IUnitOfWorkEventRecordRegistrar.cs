using Microsoft.EntityFrameworkCore.ChangeTracking;
using CompanyName.Ddd.Domain.EventBus;

namespace CompanyName.Uow
{
    public interface IUnitOfWorkEventRecordRegistrar
    {
        DispatchReport PrepareReport(IEnumerable<EntityEntry> entries);

        Task RegisterAsync(DispatchReport dispatchReport, CancellationToken cancellationToken = default);
    }
}