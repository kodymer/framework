using CompanyName.EventBus.Abstracts;

namespace CompanyName.EventBus
{
    public interface IEventHandlerInvoker
    {
        Task InvokeAsync(IEventHandler eventHandler, Type @event, object eventData);
    }
}
