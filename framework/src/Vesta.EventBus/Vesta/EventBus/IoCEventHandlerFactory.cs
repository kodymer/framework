using Microsoft.Extensions.DependencyInjection;
using Vesta.EventBus.Abstracts;

namespace Vesta.EventBus
{
    public class IoCEventHandlerFactory
    {
        private readonly Type _eventHandlerType;
        private readonly IServiceProvider _serviceProvider;

        public IoCEventHandlerFactory(IServiceProvider serviceProvider, Type eventHandlerType)
        {
            _eventHandlerType = eventHandlerType;
            _serviceProvider = serviceProvider;
        }

        public IEventHandler CreateEventHandler()
        {
            return _serviceProvider.GetRequiredService(_eventHandlerType) as IEventHandler;
        }
    }
}
