using Vesta.EventBus.Abstracts;

namespace Vesta.Uow
{
    public interface IUnitOfWorkEventPublishingStore
    {

        Task PushAsync(UnitOfWorkEventPublishing publishing);

        Task<UnitOfWorkEventPublishing> PopAsync();

        IEnumerable<UnitOfWorkEventPublishing> Get(Func<(UnitOfWorkEventPublishing, long), bool> predicate = null);
    }
}