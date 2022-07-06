using Vesta.EventBus.Abstracts;

namespace Vesta.Uow
{
    public interface IUnitOfWorkEventPublishingManager
    {
        Task<UnitOfWorkEventPublishing> CreateAndInsertAsync(IEventBus publisher, UnitOfWorkEventRecord eventRecord, long? customPriority = null, CancellationToken cancellationToken = default);
        
        Task InsertAsync(UnitOfWorkEventPublishing publishing, CancellationToken cancellationToken = default);

        Task PublishAllAsync(CancellationToken cancellationToken = default);

    }
}