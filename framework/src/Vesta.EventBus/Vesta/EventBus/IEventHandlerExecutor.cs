using Vesta.EventBus.Abstracts;

namespace Vesta.EventBus
{
    public delegate void EventHandlerMethodExecutorAsync(IEventHandler target, object parameter);

    internal interface IEventHandlerExecutor
    {
        EventHandlerMethodExecutorAsync ExecutorAsync { get; }
    }
}
