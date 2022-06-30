using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: 
    InternalsVisibleTo("Vesta.EntityFrameworkCore"),
    InternalsVisibleTo("Vesta.Ddd.Domain")]

namespace Vesta.Ddd.Domain.EventBus
{
    internal interface IGenerateIntegrationEvents
    {
        internal ImmutableList<EventRecord> GetDistributedEvents();

        void ClearDistributedEvents();

        void AddDistributedEvent(object @event);
    }

}
