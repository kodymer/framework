using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Vesta.EventBus.Abstracts;
using Vesta.Security.Claims;
using Vesta.ServiceBus.Azure;

namespace Vesta.EventBus.Azure
{
    public class AzureEventBus : EventBusBase<AzureServiceBusMessage, AzureServiceBusReceivedMessage>, IDistributedEventBus
    {

        private readonly AzureEventBusOptions _eventBusOptions;
        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;
        private readonly IAzureServiceBusMessageConsumer _serviceBusMessageConsumer;
        private readonly IPublisherPool _publisherPool;

        public AzureEventBus(
            IServiceProvider serviceProvider,
            ICurrentPrincipalAccessor currentPrincipalAccessor,
            IOptions<AzureEventBusOptions> eventBusOptions,
            IntegrationEventHandlerTypeProvider integrationEventHandlerTypeProvider,
            IAzureServiceBusMessageConsumer serviceBusMessageConsumer,
            IPublisherPool publisherPool,
            IEventHandlerInvoker eventHandlerInvoker)
            : base(serviceProvider, integrationEventHandlerTypeProvider, eventHandlerInvoker)
        {
            _eventBusOptions = eventBusOptions.Value;
            _currentPrincipalAccessor = currentPrincipalAccessor;
            _serviceBusMessageConsumer = serviceBusMessageConsumer;
            _publisherPool = publisherPool;
        }

        protected override void InitializeListener()
        {
            _serviceBusMessageConsumer.Initialize(_eventBusOptions.ConnectionString, _eventBusOptions.TopicName, _eventBusOptions.SubscriberName);
            _serviceBusMessageConsumer.OnMessageReceived(ProcessEventAsync);
        }

        protected override Task ProcessEventAsync(AzureServiceBusReceivedMessage message)
        {
            Logger.LogInformation($"Precessing metadata message.");

            Logger.LogDebug("Get metadata from message {MessageId}.", message.MessageId);

            if (message.ApplicationProperties.TryGetValue("Principal", out var value)
                && value is string principal)
            {
                Thread.CurrentPrincipal = ClaimsPrincipalFormatter.Deserialize(principal);
            }

            return base.ProcessEventAsync(message);
        }

        protected override AzureServiceBusMessage CreateMessage(string eventName, byte[] body, Guid? eventId)
        {
            var message = base.CreateMessage(eventName, body, eventId);

            Logger.LogDebug("Set metadata to message {MessageId}.", message.MessageId);

            if ((_currentPrincipalAccessor.Principal?.Identity?.IsAuthenticated).GetValueOrDefault())
            {
                message.ApplicationProperties["Principal"] = ClaimsPrincipalFormatter.Serialize(_currentPrincipalAccessor.Principal);
            }

            return message;

        }

        protected override async Task PublishAsync(string eventName, byte[] body, Guid? eventId)
        {
            var message = CreateMessage(eventName, body, eventId);

            try
            {
                var sender = _publisherPool.GetSender(_eventBusOptions.ConnectionString, _eventBusOptions.TopicName);
                await sender.SendMessageAsync(message);

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
