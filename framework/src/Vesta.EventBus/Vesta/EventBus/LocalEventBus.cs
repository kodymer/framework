using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using Vesta.EventBus.Abstracts;
using Vesta.ServiceBus.Local;

namespace Vesta.EventBus
{
    public class LocalEventBus : EventBusBase<LocalServiceBusMessage, LocalServiceBusMessage>, ILocalEventBus
    {

        private readonly ILocalServiceBusMessageConsumer _serviceBusMessageConsumer;
        private readonly ILocalServiceBusSender _publisher;

        public LocalEventBus(
            IServiceProvider serviceProvider,
            DomainEventHandlerTypeProvider domainEventHandlerTypeProvider,
            ILocalServiceBusMessageConsumer serviceBusMessageConsumer,
            ILocalServiceBusSender publisher,
            IEventHandlerInvoker eventHandlerInvoker)
            : base(serviceProvider, domainEventHandlerTypeProvider, eventHandlerInvoker)
        {
            _serviceBusMessageConsumer = serviceBusMessageConsumer;
            _publisher = publisher;
        }

        protected override void InitializeListener()
        {
            _serviceBusMessageConsumer.Initialize();
            _serviceBusMessageConsumer.OnMessageReceived(ProcessEventAsync);
        }

        protected override async Task PublishAsync(string eventName, byte[] body, Guid? eventId)
        {
            var message = CreateMessage(eventName, body, eventId);

            try
            {

                await _publisher.SendMessageAsync(message);

                Logger.LogInformation("Published message: {Message}.", JsonSerializer.Serialize(message));
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Could not publish message.({MessageId}).", message.MessageId);
                throw;
            }
        }
    }
}
