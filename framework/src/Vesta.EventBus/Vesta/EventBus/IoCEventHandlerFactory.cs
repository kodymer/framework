using Microsoft.Extensions.DependencyInjection;
using Vesta.EventBus.Abstracts;

namespace Vesta.EventBus
{
    public class IoCEventHandlerFactory
    {
        private readonly Type _eventHandlerType;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public IoCEventHandlerFactory(IServiceScopeFactory serviceScopeFactory, Type eventHandlerType)
        {
            _eventHandlerType = eventHandlerType;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IEventHandler CreateEventHandler()
        {
            using(var scope = _serviceScopeFactory.CreateScope())
            {
                return scope.ServiceProvider.GetRequiredService(_eventHandlerType) as IEventHandler;
            }
        }
    }
}
