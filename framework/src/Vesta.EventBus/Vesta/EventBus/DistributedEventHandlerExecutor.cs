using Vesta.EventBus.Abstracts;

namespace Vesta.EventBus
{
    internal class DistributedEventHandlerExecutor<TEvent> : IEventHandlerExecutor
        where TEvent : class
    {
        public EventHandlerMethodExecutorAsync ExecutorAsync => 
            (target, parameter) => target.As<IDistributedEventHandler<TEvent>>().HandleEventAsync(parameter.As<TEvent>());

        public void ExcuteAsync(IEventHandler target, TEvent parameter)
        {
            ExecutorAsync(target, parameter);
        } 
    }
}
