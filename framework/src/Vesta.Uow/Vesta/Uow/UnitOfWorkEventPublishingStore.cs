using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.EventBus.Abstracts;

namespace Vesta.Uow
{
    public class UnitOfWorkEventPublishingStore : IUnitOfWorkEventPublishingStore
    {
        private readonly PriorityQueue<UnitOfWorkEventPublishing, long> _repository; // TODO: Convert true repository

        public UnitOfWorkEventPublishingStore()
        {
            _repository = new PriorityQueue<UnitOfWorkEventPublishing, long>();
        }

        public Task PushAsync(UnitOfWorkEventPublishing publishing)
        {
            _repository.Enqueue(publishing, publishing.Priority);

            return Task.CompletedTask;
        }

        public Task<UnitOfWorkEventPublishing> PopAsync()
        {
            var publishing = _repository.Dequeue();

            return Task.FromResult(publishing);
        }

        public IEnumerable<UnitOfWorkEventPublishing> Get(Func<(UnitOfWorkEventPublishing, long), bool> predicate = null)
        {
            var publishing = (predicate is null) ? 
                _repository.UnorderedItems.Select(q => q.Element) :
                _repository.UnorderedItems.Where(predicate).Select(q => q.Item1);

            return publishing;
        }
    }
}
