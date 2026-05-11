using CommunityToolkit.Diagnostics;
using CompanyName.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CompanyName.Uow
{
    internal class UnitOfWorkEventDispatcher : IEventDispatcher
    {
        protected ILogger<UnitOfWorkEventDispatcher> Logger { get; set; }

        private object _prioritizationLock = new object();
        private readonly IEventStore _store;

        public UnitOfWorkEventDispatcher(
            IEventStore store)
        {
            _store = store;

            Logger = NullLogger<UnitOfWorkEventDispatcher>.Instance;
        }

        public async Task<UnitOfWorkEventPublishing> CreateAndInsertAsync(IEventBus publisher, UnitOfWorkEventRecord eventRecord, long? customPriority = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(publisher);
            Guard.IsNotNull(eventRecord);

            if (customPriority is not null)
            {
                Guard.IsGreaterThan(customPriority.Value, 0);
            }
            else
            {
                lock (_prioritizationLock)
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
            Guard.IsNotNull(publishing);

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
