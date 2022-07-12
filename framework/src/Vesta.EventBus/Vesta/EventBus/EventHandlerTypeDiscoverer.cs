using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vesta.EventBus.Abstracts;

namespace Vesta.EventBus
{
    internal static class EventHandlerTypeDiscoverer
    {

        public static bool TryDiscoverEventType(Type @interface, out Type @event)
        {
            @event = null;

            var genericArguments = @interface.GetGenericArguments();
            if (genericArguments.Length == 1)
            {
                @event = genericArguments[0];

                return true;
            }

            return false;
        }

        public static bool TryDiscoverEventHandlerInterface(Type eventHandler, out Type @interface)
        {
            @interface = null;

            var contracts = eventHandler.GetInterfaces();
            foreach (var contract in contracts)
            {
                if (typeof(IEventHandler).GetTypeInfo().IsAssignableFrom(contract))
                {
                    @interface = contract;

                    return true;
                }
            }

            return false;
        }
    }
}
