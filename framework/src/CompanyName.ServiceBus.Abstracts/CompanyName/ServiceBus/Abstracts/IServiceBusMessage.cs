using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.ServiceBus.Abstracts
{
    public interface IServiceBusMessage
    {
        string Subject { get; set; }

        string MessageId { get; set; }

        BinaryData Body { get; set; }
    }
}
