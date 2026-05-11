using CommunityToolkit.Diagnostics;
using CompanyName.EventBus.Abstractions;

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

        public EventHandlerOptions Add(Type eventHandlerType)
        {
            Guard.IsTrue(eventHandlerType.GetInterfaces().Any(@interface => @interface == typeof(IEventHandler)));

            if (!_handlers.ContainsKey(eventHandlerType.FullName))
            {
                _handlers.Add(eventHandlerType.FullName, eventHandlerType);
            }

            return this;
        }

        internal IEnumerable<Type> GetAll()
        {
            return _handlers.Values;
        }
    }
}
