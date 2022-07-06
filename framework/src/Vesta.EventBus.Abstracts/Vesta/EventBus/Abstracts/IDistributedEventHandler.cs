using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.EventBus.Abstracts
{
    public interface IDistributedEventHandler<TArgs> : IEventHandler
        where TArgs : class
    {
        Task HandleEventAsync(TArgs args);
    }
}
