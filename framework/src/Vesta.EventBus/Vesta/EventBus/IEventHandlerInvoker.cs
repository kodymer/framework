using Vesta.EventBus.Abstracts;

namespace Vesta.EventBus
{
    public interface IEventHandlerInvoker
    {
        Task InvokeAsync(IEventHandler eventHandler, Type @event, object eventData);
    }
}
