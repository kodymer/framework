using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyName.Messaging.Abstractions;

namespace CompanyName.Messaging.InProcess
{
    public class LocalServiceBusMessage : IServiceBusMessage, IServiceBusReceivedMessage
    {
        public string Subject { get; set; }

        public string MessageId { get; set; }

        public BinaryData Body { get; set; }

    }
}
