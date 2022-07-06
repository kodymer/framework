using Ardalis.GuardClauses;
using Vesta.EventBus.Abstracts;

namespace Vesta.Uow
{
    internal class UnitOfWorkEventPublishingManager : IUnitOfWorkEventPublishingManager
    {
        private object _prioritizationThreadSafeLockFlag = new object();
        private readonly IUnitOfWorkEventPublishingStore _store;

        public UnitOfWorkEventPublishingManager(
            IUnitOfWorkEventPublishingStore store)
        {
            _store = store;
        }

        public async Task<UnitOfWorkEventPublishing> CreateAndInsertAsync(IEventBus publisher, UnitOfWorkEventRecord eventRecord, long? customPriority = null, CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(publisher, nameof(publisher));
            Guard.Against.Null(eventRecord, nameof(eventRecord));

            if (customPriority is not null)
            {
                Guard.Against.NegativeOrZero(customPriority.Value, nameof(customPriority));
            }
            else
            {
                lock (_prioritizationThreadSafeLockFlag)
                {
                    customPriority = _store.Get().Select(p => p.Priority).Max() + 1;
                }
            }

            var publishing = new UnitOfWorkEventPublishing(publisher, eventRecord, (long)customPriority);
            await InsertAsync(publishing);

            return publishing;

        }

        public virtual async Task InsertAsync(UnitOfWorkEventPublishing publishing, CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(publishing, nameof(publishing));

            await _store.PushAsync(publishing);
        }

        public virtual async Task PublishAllAsync(CancellationToken cancellationToken = default)
        {
            var allPublishing = _store.Get();
            if (allPublishing.Any())
            {
                var count = allPublishing.Count();
                for (int i = 0; i <= count - 1; i++)
                {
                    var publishing = await _store.PopAsync();
                    await publishing.SendAsync();
                }
            }
        }
    }
}
