using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.EventBus.Abstracts;

namespace Vesta.EventBus
{
    public class IntegrationEventHandlerTypeProvider : IEventHandlerTypeProvider
    {
        private readonly EventHandlerOptions _eventHandlerOptions;

        public IntegrationEventHandlerTypeProvider(
            IOptions<EventHandlerOptions> eventHandlerOptions)
        {
            _eventHandlerOptions = eventHandlerOptions.Value;
        }


        public IEnumerable<Type> GetAll()
        {
            Func<Type, bool> predicate = h =>
                    EventHandlerTypeDiscoverer.TryDiscoverEventHandlerInterface(h, out var @interface) &&
                    @interface.Name == typeof(IIntegrationEventHandler<>).Name;

            return _eventHandlerOptions.GetAll().Where(predicate);
        }
    }
}
