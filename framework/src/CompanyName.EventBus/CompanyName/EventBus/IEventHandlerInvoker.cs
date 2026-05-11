using CompanyName.EventBus.Abstractions;

namespace CompanyName.EventBus
{
    public interface IEventHandlerInvoker
    {
        Task InvokeAsync(IEventHandler eventHandler, Type @event, object eventData);
    }
}
