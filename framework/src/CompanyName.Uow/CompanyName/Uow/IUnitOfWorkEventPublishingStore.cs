using CompanyName.EventBus.Abstracts;

namespace CompanyName.Uow
{
    public interface IUnitOfWorkEventPublishingStore
    {

        Task PushAsync(UnitOfWorkEventPublishing publishing, CancellationToken cancellationToken = default);

        Task<UnitOfWorkEventPublishing> PopAsync(CancellationToken cancellationToken = default);

        IEnumerable<UnitOfWorkEventPublishing> Get(Func<(UnitOfWorkEventPublishing, long), bool> predicate = null);
    }
}