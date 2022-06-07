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
    public class AzureEventBus : EventBusBase, IDistributedEventBus
    {

        private readonly AzureEventBusOptions _eventBusOptions;
        private readonly EventHandlerOptions _eventHandlerOptions;
        private readonly IAzureServiceBusMessageConsumer _serviceBusMessageConsumer;
        private readonly IEventHandlerInvoker _eventHandlerInvoker;
        private readonly IPublisherPool _publisherPool;

        public AzureEventBus(
            IServiceProvider serviceProvider,
            IOptions<AzureEventBusOptions> eventBusOptions,
            IOptions<EventHandlerOptions> eventHandlerOptions,
            IAzureServiceBusMessageConsumer serviceBusMessageConsumer,
            IPublisherPool publisherPool,
            IEventHandlerInvoker eventHandlerInvoker)
            : base(serviceProvider)
        {
            _eventBusOptions = eventBusOptions.Value;
            _eventHandlerOptions = eventHandlerOptions.Value;
            _serviceBusMessageConsumer = serviceBusMessageConsumer;
            _eventHandlerInvoker = eventHandlerInvoker;
            _publisherPool = publisherPool;
        }

        public void Initialize()
        {
            Logger.LogInformation($"Initializing event bus service.");

            var eventHandlers = _eventHandlerOptions.GetAll();
            if (eventHandlers.Any())
            {
                _serviceBusMessageConsumer.Initialize(_eventBusOptions.ConnectionString, _eventBusOptions.TopicName, _eventBusOptions.SubscriberName);
                _serviceBusMessageConsumer.OnMessageReceived(ProcessEventAsync);

                SubscribeHandlers(eventHandlers);
            }
        }

        private async Task ProcessEventAsync(ServiceBusReceivedMessage message)
        {
            Logger.LogInformation($"Precessing message: {JsonSerializer.Serialize(message)}");

            var eventName = message.Subject;
            if (eventName is null)
            {
                return;
            }

            Logger.LogDebug($"Event subject: {eventName}.");

            var @event = EventTypes.GetOrDefault(eventName);
            if (@event is null)
            {
                return;
            }

            Logger.LogDebug($"Event type: {@event.FullName}.");

            var eventData = JsonSerializer.Deserialize(Encoding.UTF8.GetString(message.Body.ToArray()), @event);

            Logger.LogDebug($"Event data: {JsonSerializer.Serialize(eventData)}.");

            await TriggerEventHandler(@event, eventData);
        }

        private async Task TriggerEventHandler(Type @event, object eventData)
        {
            Logger.LogInformation($"Trigger {@event.Name} event handler.");

            if (EventHandlerFactories.TryGetValue(@event, out var factory))
            {
                var eventHandler = factory.CreateEventHandler();

                Logger.LogDebug($"Invoking event handler: {eventHandler.GetType().Name}.");

                await _eventHandlerInvoker.InvokeAsync(eventHandler, @event, eventData);
            }
        }

        protected override async Task PublishAsync<TEvent>(string eventName, TEvent @event)
             where TEvent : class
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));

            await PublishAsync(eventName, body, null);
        }

        protected async Task PublishAsync(string eventName, byte[] body, Guid? eventId)
        {
            var message = new ServiceBusMessage(body);

            if (!string.IsNullOrWhiteSpace(eventName))
            {
                message.Subject = eventName;
            }

            if (string.IsNullOrWhiteSpace(message.MessageId))
            {
                message.MessageId = (eventId ?? Guid.NewGuid()).ToString("N");
            }

            Logger.LogDebug("Message Id: {MessageId}.", message.MessageId);

            var sender = _publisherPool.GetSender(_eventBusOptions.ConnectionString, _eventBusOptions.TopicName);
            await sender.SendMessageAsync(message);

            Logger.LogInformation("Published message: {Message}.", JsonSerializer.Serialize(message));
        }
    }
}
