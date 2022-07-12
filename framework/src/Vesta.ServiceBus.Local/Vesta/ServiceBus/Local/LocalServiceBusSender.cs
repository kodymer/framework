using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.ServiceBus.Local
{
    public class LocalServiceBusSender : ILocalServiceBusSender
    {
        private readonly LocalServiceBusQueue _queue;

        public LocalServiceBusSender(LocalServiceBusQueue queue)
        {
            _queue = queue;
        }

        public Task SendMessageAsync(LocalServiceBusMessage message)
        {
            Guard.Against.Null(message, nameof(message));

            _queue.Enqueue(message);

            return Task.CompletedTask;
        }
    }
}
