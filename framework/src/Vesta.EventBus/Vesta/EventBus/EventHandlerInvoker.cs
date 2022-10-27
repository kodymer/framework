using Ardalis.GuardClauses;
using Vesta.EventBus.Abstracts;

namespace Vesta.EventBus
{
    public class EventHandlerInvoker : IEventHandlerInvoker
    {
        public async Task InvokeAsync(IEventHandler eventHandler, Type @event, object eventData)
        {
            Guard.Against.Null(eventHandler, nameof(eventHandler));
            Guard.Against.Null(@event, nameof(@event));
            Guard.Against.Null(eventData, nameof(eventData));

            IEventHandlerMethodExecutor eventHandlerExecutor = null;

            if (typeof(IIntegrationEventHandler<>).MakeGenericType(@event).IsInstanceOfType(eventHandler))
            {
                eventHandlerExecutor = (IEventHandlerMethodExecutor)Activator.CreateInstance(typeof(EventHandlerMethodExecutor<,>)
                    .MakeGenericType(@event, typeof(IIntegrationEventHandler<>).MakeGenericType(@event)));
            }

            if (typeof(IDomainEventHandler<>).MakeGenericType(@event).IsInstanceOfType(eventHandler))
            {
                eventHandlerExecutor = (IEventHandlerMethodExecutor)Activator.CreateInstance(typeof(EventHandlerMethodExecutor<,>)
                    .MakeGenericType(@event, typeof(IDomainEventHandler<>).MakeGenericType(@event)));
            }

            if (eventHandlerExecutor is not null)
            {
                await eventHandlerExecutor.ExecutorAsync(eventHandler, eventData);
            }
            else
                throw new NotSupportedException("The event handler is not supported!");
        }
    }
}
