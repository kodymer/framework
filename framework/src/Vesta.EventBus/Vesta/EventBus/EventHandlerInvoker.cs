using Ardalis.GuardClauses;
using Vesta.EventBus.Abstracts;

namespace Vesta.EventBus
{
    public class EventHandlerInvoker : IEventHandlerInvoker
    {
        public Task InvokeAsync(IEventHandler eventHandler, Type @event, object eventData)
        {
            Guard.Against.Null(eventHandler, nameof(eventHandler));
            Guard.Against.Null(@event, nameof(@event));
            Guard.Against.Null(eventData, nameof(eventData));

            IEventHandlerExecutor eventHandlerExecutor = null;

            if (typeof(IIntegrationEventHandler<>).MakeGenericType(@event).IsInstanceOfType(eventHandler))
            {
                eventHandlerExecutor = (IEventHandlerExecutor)Activator.CreateInstance(typeof(EventHandlerExecutor<,>)
                    .MakeGenericType(@event, typeof(IIntegrationEventHandler<>).MakeGenericType(@event)));
            }

            if (typeof(IDomainEventHandler<>).MakeGenericType(@event).IsInstanceOfType(eventHandler))
            {
                eventHandlerExecutor = (IEventHandlerExecutor)Activator.CreateInstance(typeof(EventHandlerExecutor<,>)
                    .MakeGenericType(@event, typeof(IDomainEventHandler<>).MakeGenericType(@event)));
            }

            if (eventHandlerExecutor is not null)
            {
                eventHandlerExecutor.ExecutorAsync(eventHandler, eventData);

                return Task.CompletedTask;
            }
            else
                throw new NotSupportedException("The event handler is not supported!");
        }
    }
}
