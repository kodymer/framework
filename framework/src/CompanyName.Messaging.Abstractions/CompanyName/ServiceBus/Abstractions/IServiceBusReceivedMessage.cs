using System.Security.Principal;

namespace CompanyName.ServiceBus.Abstractions
{
    public interface IServiceBusReceivedMessage
    {

        string Subject { get; }

        string MessageId { get; }

        BinaryData Body { get; }
    }
}
