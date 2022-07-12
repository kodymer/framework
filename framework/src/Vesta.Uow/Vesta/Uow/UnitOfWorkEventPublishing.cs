using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.EventBus.Abstracts;

namespace Vesta.Uow
{
    public class UnitOfWorkEventPublishing
    {
        public long Priority { get; }

        private readonly IEventBus _publisher;
        private readonly UnitOfWorkEventRecord _eventRecord;

        public UnitOfWorkEventPublishing(IEventBus publisher, UnitOfWorkEventRecord eventRecord, long priority)
        {
            _publisher = publisher;
            _eventRecord = eventRecord;
            Priority = priority;
        }

        public async Task SendAsync(CancellationToken cancellationToken = default)
        {
            await _publisher.PublishAsync(_eventRecord.Data);
        }
    }


}
