using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using CompanyName.ServiceBus.Abstractions;

namespace CompanyName.Messaging.AzureServiceBus
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
