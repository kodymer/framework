using CommunityToolkit.Diagnostics;
using CompanyName.EventBus.Abstractions;

namespace CompanyName.EventBus
{
    public class EventHandlerInvoker : IEventHandlerInvoker
    {
        public async Task InvokeAsync(IEventHandler eventHandler, Type @event, object eventData)
        {
            Guard.IsNotNull(eventHandler, nameof(eventHandler));
            Guard.IsNotNull(@event, nameof(@event));
            Guard.IsNotNull(eventData, nameof(eventData));

            IEventHandlerMethodExecutor eventHandlerExecutor = null;

            if (typeof(IEventHandler<>).MakeGenericType(@event).IsInstanceOfType(eventHandler))
            {
                eventHandlerExecutor = (IEventHandlerMethodExecutor)Activator.CreateInstance(typeof(EventHandlerMethodExecutor<,>)
                    .MakeGenericType(@event, typeof(IEventHandler<>).MakeGenericType(@event)));
            }

            // TO-DO: Remove block

            //if (typeof(IIntegrationEventHandler<>).MakeGenericType(@event).IsInstanceOfType(eventHandler))
            //{
            //    eventHandlerExecutor = (IEventHandlerMethodExecutor)Activator.CreateInstance(typeof(EventHandlerMethodExecutor<,>)
            //        .MakeGenericType(@event, typeof(IIntegrationEventHandler<>).MakeGenericType(@event)));
            //}

            //if (typeof(IDomainEventHandler<>).MakeGenericType(@event).IsInstanceOfType(eventHandler))
            //{
            //    eventHandlerExecutor = (IEventHandlerMethodExecutor)Activator.CreateInstance(typeof(EventHandlerMethodExecutor<,>)
            //        .MakeGenericType(@event, typeof(IDomainEventHandler<>).MakeGenericType(@event)));
            //}

            if (eventHandlerExecutor is not null)
            {
                await eventHandlerExecutor.ExecutorAsync(eventHandler, eventData);
            }
            else
                throw new NotSupportedException("The event handler is not supported!");
        }
    }
}
