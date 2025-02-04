using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.EventBus.Abstracts
{
    public interface IEventHandlerTypeProvider
    {
        IEnumerable<Type> GetAll();
    }
}
