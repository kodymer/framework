using CommunityToolkit.Diagnostics;

namespace CompanyName.ServiceBus.Local
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
            Guard.IsNotNull(message, nameof(message));

            _queue.Enqueue(message);

            return Task.CompletedTask;
        }
    }
}
