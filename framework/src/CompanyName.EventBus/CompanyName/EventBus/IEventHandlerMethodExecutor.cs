using CompanyName.EventBus.Abstractions;

namespace CompanyName.EventBus
{
    public delegate Task EventHandlerMethodExecutorAsync(IEventHandler target, object parameter);

    internal interface IEventHandlerMethodExecutor
    {
        EventHandlerMethodExecutorAsync ExecutorAsync { get; }
    }
}
