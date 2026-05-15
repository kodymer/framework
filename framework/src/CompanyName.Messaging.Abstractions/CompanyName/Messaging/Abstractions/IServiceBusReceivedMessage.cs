using System.Security.Principal;

namespace CompanyName.Messaging.Abstractions
{
    public interface IServiceBusReceivedMessage
    {

        string Subject { get; }

        string MessageId { get; }

        BinaryData Body { get; }
    }
}
