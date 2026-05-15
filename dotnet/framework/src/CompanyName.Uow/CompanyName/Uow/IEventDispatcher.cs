using CompanyName.EventBus.Abstractions;

namespace CompanyName.Uow
{
    public interface IEventDispatcher
    {
        Task<UnitOfWorkEventPublishing> CreateAndInsertAsync(IEventBus publisher, UnitOfWorkEventRecord eventRecord, long? customPriority = null, CancellationToken cancellationToken = default);
        
        Task InsertAsync(UnitOfWorkEventPublishing publishing, CancellationToken cancellationToken = default);

        Task PublishAllAsync(CancellationToken cancellationToken = default);

    }
}