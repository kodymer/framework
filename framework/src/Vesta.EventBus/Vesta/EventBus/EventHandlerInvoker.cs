using Vesta.EventBus.Abstracts;

namespace Vesta.EventBus
{
    public class EventHandlerInvoker : IEventHandlerInvoker
    {
        public Task InvokeAsync(IEventHandler eventHandler, Type @event, object eventData)
        {
            if (typeof(IDistributedEventHandler<>).MakeGenericType(@event).IsInstanceOfType(eventHandler))
            {
                var eventHandlerExecutor = (IEventHandlerExecutor)Activator.CreateInstance(typeof(DistributedEventHandlerExecutor<>).MakeGenericType(@event));
                eventHandlerExecutor.ExecutorAsync(eventHandler, eventData);
            }
            else
            {
                throw new NotSupportedException("The event handler is not supported!");
            }

            return Task.CompletedTask;
        }
    }
}
