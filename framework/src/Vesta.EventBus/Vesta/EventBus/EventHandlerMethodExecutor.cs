using Vesta.EventBus.Abstracts;

namespace Vesta.EventBus
{
    internal class EventHandlerMethodExecutor<TEvent, TEventHandler> : IEventHandlerMethodExecutor
        where TEvent : class
        where TEventHandler : class, IEventHandler<TEvent>
    {
        public EventHandlerMethodExecutorAsync ExecutorAsync => 
            (target, parameter) => target.As<TEventHandler>().HandleEventAsync(parameter.As<TEvent>());

        public void ExcuteAsync(TEventHandler target, TEvent parameter)
        {
            ExecutorAsync(target, parameter);
        } 
    }
}
