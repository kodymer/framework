using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Vesta.EventBus.Abstracts;
using Vesta.ServiceBus.Azure;

namespace Vesta.EventBus.Azure
{
    public class AzureEventBus : EventBusBase<AzureServiceBusMessage, AzureServiceBusReceivedMessage>, IDistributedEventBus
    {

        private readonly AzureEventBusOptions _eventBusOptions;
        private readonly IAzureServiceBusMessageConsumer _serviceBusMessageConsumer;
        private readonly IPublisherPool _publisherPool;

        public AzureEventBus(
            IServiceProvider serviceProvider,
            IOptions<AzureEventBusOptions> eventBusOptions,
            IntegrationEventHandlerTypeProvider integrationEventHandlerTypeProvider,
            IAzureServiceBusMessageConsumer serviceBusMessageConsumer,
            IPublisherPool publisherPool,
            IEventHandlerInvoker eventHandlerInvoker)
            : base(serviceProvider, integrationEventHandlerTypeProvider, eventHandlerInvoker)
        {
            _eventBusOptions = eventBusOptions.Value;
            _serviceBusMessageConsumer = serviceBusMessageConsumer;
            _publisherPool = publisherPool;
        }

        protected override void InitializeListener()
        {
            _serviceBusMessageConsumer.Initialize(_eventBusOptions.ConnectionString, _eventBusOptions.TopicName, _eventBusOptions.SubscriberName);
            _serviceBusMessageConsumer.OnMessageReceived(ProcessEventAsync);
        }

        protected override async Task PublishAsync(string eventName, byte[] body, Guid? eventId)
        {
            var message = CreateMessage(eventName, body, eventId);

            var sender = _publisherPool.GetSender(_eventBusOptions.ConnectionString, _eventBusOptions.TopicName);
            await sender.SendMessageAsync(message);

            Logger.LogInformation("Published message: {Message}.", JsonSerializer.Serialize(message));
        }
    }
}
