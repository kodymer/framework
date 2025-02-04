using Ardalis.GuardClauses;
using CompanyName.EventBus.Abstracts;

namespace CompanyName.EventBus
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
            Guard.Against.InvalidInput(
                eventHandler, nameof(eventHandler), 
                argument => argument.GetInterfaces().Any(@interface => @interface == typeof(IEventHandler)));

            if (!_handlers.ContainsKey(eventHandler.FullName))
            {
                _handlers.Add(eventHandler.FullName, eventHandler);
            }

            return this;
        }

        internal IEnumerable<Type> GetAll()
        {
            return _handlers.Values;
        }
    }
}
