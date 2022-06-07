using Vesta.EventBus.Abstracts;

namespace Vesta.EventBus
{
    public class EventHandlerOptions
    {
        private Dictionary<string, Type> _handlers;

        public EventHandlerOptions()
        {
            _handlers = new Dictionary<string, Type>();
        }

        public EventHandlerOptions Add<T>()
            where T : class, IEventHandler
        {
            Add(typeof(T));

            return this;
        }

        public EventHandlerOptions Add(Type eventHandler)
        {
            if (!_handlers.ContainsKey(eventHandler.FullName))
            {
                _handlers.Add(eventHandler.FullName, eventHandler);
            }

            return this;
        }

        internal List<Type> GetAll()
        {
            return _handlers.Values.ToList();
        }

    }
}
