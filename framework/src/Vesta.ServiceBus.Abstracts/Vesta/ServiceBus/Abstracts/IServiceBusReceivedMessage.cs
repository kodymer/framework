using System.Security.Principal;

namespace Vesta.ServiceBus.Abstracts
{
    public interface IServiceBusReceivedMessage
    {

        string Subject { get; }

        string MessageId { get; }

        BinaryData Body { get; }
    }
}
