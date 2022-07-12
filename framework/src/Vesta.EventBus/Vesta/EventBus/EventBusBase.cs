using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Vesta.Ddd.Domain.EventBus;
using Vesta.EventBus.Abstracts;
using Vesta.ServiceBus.Abstracts;

[assembly: InternalsVisibleTo("Vesta.Ddd.Application.EventBus")]

namespace Vesta.EventBus
{
    public abstract class EventBusBase<TSentMessage, TReceivedMessage>
        where TSentMessage : class, IServiceBusMessage
        where TReceivedMessage : class, IServiceBusReceivedMessage
    {

        protected ILogger Logger => _logger ??= LoggerFactory.CreateLogger(GetType().FullName);
        protected ConcurrentDictionary<string, Type> EventTypes { get; }
        protected ConcurrentDictionary<Type, IoCEventHandlerFactory> EventHandlerFactories { get; }
        protected IEventHandlerTypeProvider EventHandlerTypeProvider { get; }
        protected IEventHandlerInvoker EventHandlerInvoker { get; }
        protected IServiceProvider ServiceProvider { get; }

        private ILogger _logger;
        private ILoggerFactory LoggerFactory => ServiceProvider.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;

        public EventBusBase(
            IServiceProvider serviceProvider,
            IEventHandlerTypeProvider eventHandlerTypeProvider,
            IEventHandlerInvoker eventHandlerInvoker
        )
        {
            EventTypes = new ConcurrentDictionary<string, Type>();
            EventHandlerFactories = new ConcurrentDictionary<Type, IoCEventHandlerFactory>();

            EventHandlerInvoker = eventHandlerInvoker;
            ServiceProvider = serviceProvider;
            EventHandlerTypeProvider = eventHandlerTypeProvider;
        }

        public async Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class
        {
            Logger.LogInformation("Publishing message. Type: {EventType} - Data: {Data}.", typeof(TEvent).FullName, JsonSerializer.Serialize(@event));

            var eventName = EventNameAttribute.GetOrDefault(@event.GetType());

            Logger.LogDebug("Message subject: {Subject}", eventName);

            await PublishAsync(eventName, @event);
        }

        internal protected virtual void Initialize()
        {
            Logger.LogInformation($"Initializing event bus service.");

            var eventHandlers = EventHandlerTypeProvider.GetAll();
            if (eventHandlers.Any())
            {
                InitializeListener();

                SubscribeHandlers(eventHandlers);
            }
        }

        protected abstract void InitializeListener();

        protected virtual async Task ProcessEventAsync(TReceivedMessage message)
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

            var body = Encoding.UTF8.GetString(message.Body.ToArray());
            var eventData = JsonSerializer.Deserialize(body, @event);

            Logger.LogDebug($"Event data: {JsonSerializer.Serialize(eventData)}.");

            await TriggerEventHandler(@event, eventData);
        }

        protected virtual async Task TriggerEventHandler(Type @event, object eventData)
        {
            Logger.LogInformation($"Trigger {@event.Name} event handler.");

            if (EventHandlerFactories.TryGetValue(@event, out var factory))
            {
                var eventHandler = factory.CreateEventHandler();

                Logger.LogDebug($"Invoking event handler: {eventHandler.GetType().Name}.");

                await EventHandlerInvoker.InvokeAsync(eventHandler, @event, eventData);
            }
        }

        protected virtual void Subscribe(Type @event, IoCEventHandlerFactory eventHandlerFactory)
        {
            var eventName = @event.FullName;

            Logger.LogInformation("The event has been recorded as {EventName}", eventName);

            EventTypes.GetOrAdd(eventName, @event);
            EventHandlerFactories.GetOrAdd(@event, eventHandlerFactory);
        }

        protected virtual void SubscribeHandlers(IEnumerable<Type> handlers)
        {
            foreach (var handler in handlers)
            {
                Logger.LogInformation($"Subscribing event handler: {handler.Name}.");

                if (EventHandlerTypeDiscoverer.TryDiscoverEventHandlerInterface(handler, out var @interface)
                    && EventHandlerTypeDiscoverer.TryDiscoverEventType(@interface, out var @event))
                {
                    Logger.LogDebug($"Found event type: {@event.Name}.");

                    Subscribe(@event, new IoCEventHandlerFactory(ServiceProvider, @interface));
                }
            }
        }

        protected virtual TSentMessage CreateMessage(string eventName, byte[] body, Guid? eventId)
        {
            var message = (TSentMessage)Activator.CreateInstance(typeof(TSentMessage));

            if (!string.IsNullOrWhiteSpace(eventName))
            {
                message.Subject = eventName;
            }

            if (string.IsNullOrWhiteSpace(message.MessageId))
            {
                message.MessageId = (eventId ?? Guid.NewGuid()).ToString("N");
            }

            message.Body = BinaryData.FromBytes(body);

            Logger.LogDebug("Message {MessageId} Created!.", message.MessageId);

            return message;
        }

        protected virtual async Task PublishAsync<TEvent>(string eventName, TEvent @event)
            where TEvent : class
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
            await PublishAsync(eventName, body, null);
        }

        protected abstract Task PublishAsync(string eventName, byte[] body, Guid? eventId);
    }
}