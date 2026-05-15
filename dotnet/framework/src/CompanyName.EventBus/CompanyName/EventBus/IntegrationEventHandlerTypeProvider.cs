using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyName.EventBus.Abstractions;

namespace CompanyName.EventBus
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
                    @interface.Name == typeof(IEventHandler<>).Name;

            return _eventHandlerOptions.GetAll().Where(predicate);
        }
    }
}
