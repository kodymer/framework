using CompanyName.EventBus.Abstractions;

namespace CompanyName.Uow
{
    public interface IEventStore
    {

        Task PushAsync(UnitOfWorkEventPublishing publishing, CancellationToken cancellationToken = default);

        Task<UnitOfWorkEventPublishing> PopAsync(CancellationToken cancellationToken = default);

        IEnumerable<UnitOfWorkEventPublishing> Get(Func<(UnitOfWorkEventPublishing, long), bool> predicate = null);
    }


}