using Vesta.EventBus.Abstracts;

namespace Vesta.EventBus
{
    public delegate Task EventHandlerMethodExecutorAsync(IEventHandler target, object parameter);

    internal interface IEventHandlerMethodExecutor
    {
        EventHandlerMethodExecutorAsync ExecutorAsync { get; }
    }
}
