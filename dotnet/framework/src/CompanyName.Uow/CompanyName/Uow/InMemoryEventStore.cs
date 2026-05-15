using CompanyName.Uow;

namespace CompanyName.Uow
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly PriorityQueue<UnitOfWorkEventPublishing, long> _queue;

        public InMemoryEventStore()
        {
            _queue = new PriorityQueue<UnitOfWorkEventPublishing, long>();
        }

        public Task PushAsync(UnitOfWorkEventPublishing publishing, CancellationToken cancellationToken = default)
        {
            _queue.Enqueue(publishing, publishing.Priority);

            return Task.CompletedTask;
        }

        public Task<UnitOfWorkEventPublishing> PopAsync(CancellationToken cancellationToken = default)
        {
            var publishing = _queue.Dequeue();

            return Task.FromResult(publishing);
        }

        public IEnumerable<UnitOfWorkEventPublishing> Get(Func<(UnitOfWorkEventPublishing, long), bool> predicate = null)
        {
            var publishing = (predicate is null) ?
                _queue.UnorderedItems.Select(q => q.Element) :
                _queue.UnorderedItems.Where(predicate).Select(q => q.Item1);

            return publishing;
        }
    }
}
