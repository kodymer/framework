using Vesta.EventBus.Abstracts;

namespace Vesta.Uow
{
    public interface IUnitOfWorkEventPublishingManager
    {
        Task CreateAsync(UnitOfWorkEventPublishing publishing, CancellationToken cancellationToken = default);

        Task PublishAllAsync(CancellationToken cancellationToken = default);

    }
}