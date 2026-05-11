using CompanyName.EventBus.Abstractions;

namespace CompanyName.EventBus
{
    public class EventHandlerDisposeWrapper : IDisposable
    {
        public IEventHandler EventHandler { get; }

        private readonly Action _disposeAction;

        public EventHandlerDisposeWrapper(IEventHandler eventHandler, Action disposeAction = null)
        {
            EventHandler = eventHandler;

            _disposeAction = disposeAction;
        }


        public void Dispose()
        {
            _disposeAction?.Invoke();
        }
    }
}
