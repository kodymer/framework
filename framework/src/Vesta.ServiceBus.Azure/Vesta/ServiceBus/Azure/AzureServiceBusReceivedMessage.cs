using Ardalis.GuardClauses;
using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.ServiceBus.Abstracts;

namespace Vesta.ServiceBus.Azure
{
    public class AzureServiceBusReceivedMessage : IServiceBusReceivedMessage
    {
        private readonly ServiceBusReceivedMessage _message;

        public AzureServiceBusReceivedMessage(ServiceBusReceivedMessage message)
        {
            _message = message;
        }

        public string Subject => _message.Subject;

        public string MessageId => _message.MessageId;

        public BinaryData Body => _message.Body;
    }
}
