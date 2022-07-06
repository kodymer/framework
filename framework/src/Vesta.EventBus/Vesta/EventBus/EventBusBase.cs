using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using Vesta.EventBus.Abstracts;
using Vesta.Ddd.Domain.EventBus;

namespace Vesta.EventBus
{
    public abstract class EventBusBase
    {

        protected ILogger Logger => _logger ??= LoggerFactory.CreateLogger(GetType().FullName);
        protected ConcurrentDictionary<string, Type> EventTypes { get; }
        protected ConcurrentDictionary<Type, IoCEventHandlerFactory> EventHandlerFactories { get; }

        private ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private ILoggerFactory LoggerFactory => _serviceProvider.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;

        public EventBusBase(IServiceProvider serviceProvider)
        {
            EventTypes = new ConcurrentDictionary<string, Type>();
            EventHandlerFactories = new ConcurrentDictionary<Type, IoCEventHandlerFactory>();

            _serviceProvider = serviceProvider;
        }

        internal protected abstract void Initialize();

        protected virtual void Subscribe(Type @event, IoCEventHandlerFactory eventHandlerFactory)
        {
            var eventName = @event.FullName;

            Logger.LogInformation("The event has been recorded as {EventName}", eventName);

            EventTypes.GetOrAdd(eventName, @event);
            EventHandlerFactories.GetOrAdd(@event, eventHandlerFactory);
        }

        protected virtual void SubscribeHandlers(List<Type> handlers)
        {
            foreach (var handler in handlers)
            {
                Logger.LogInformation($"Subscribing event handler: { handler.Name }.");

                if (TryDiscoverEventHandlerInterface(handler, out var @interface)
                    && TryDiscoverEventType(@interface, out var @event))
                {
                    Logger.LogDebug($"Found event type: { @event.Name }.");

                    Subscribe(@event, new IoCEventHandlerFactory(_serviceProvider, @interface));
                }
            }
        }

        public static bool TryDiscoverEventType(Type @interface, out Type @event)
        {
            @event = null;

            var genericArguments = @interface.GetGenericArguments();
            if (genericArguments.Length == 1)
            {
                @event = genericArguments[0];

                return true;
            }

            return false;
        }

        public static bool TryDiscoverEventHandlerInterface(Type eventHandler, out Type @interface) 
        {
            @interface = null;

            var contracts = eventHandler.GetInterfaces();
            foreach (var contract in contracts)
            {
                if (typeof(IEventHandler).GetTypeInfo().IsAssignableFrom(contract))
                {
                    @interface = contract;

                    return true;
                }
            }
            
            return false;
        }

        public async Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class
        {
            Logger.LogInformation("Publishing message. Type: {EventType} - Data: {Data}.", typeof(TEvent).FullName, JsonSerializer.Serialize(@event));

            var eventName = EventNameAttribute.GetOrDefault(@event.GetType());

            Logger.LogDebug("Message subject: {Subject}", eventName);

            await PublishAsync(eventName, @event);
        }

        protected abstract Task PublishAsync<TEvent>(string eventName, TEvent @event)
            where TEvent : class;
    }
}