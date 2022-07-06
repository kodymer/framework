using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.ServiceBus.Abstracts;

namespace Vesta.ServiceBus.Azure
{
    public class AzureServiceBusMessage : ServiceBusMessage, IServiceBusMessage
    {
        public AzureServiceBusMessage()
        {

        }

        public AzureServiceBusMessage(BinaryData body) : base(body)
        {

        }

        public AzureServiceBusMessage(string body) : base(body)
        {
        }

        public AzureServiceBusMessage(ReadOnlyMemory<byte> body) : base(body)
        {

        }

        public AzureServiceBusMessage(ServiceBusReceivedMessage message)
            : base(message)
        {

        }
    }
}
