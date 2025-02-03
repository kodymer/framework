using System.Security.Principal;

namespace CompanyName.ServiceBus.Abstracts
{
    public interface IServiceBusReceivedMessage
    {

        string Subject { get; }

        string MessageId { get; }

        BinaryData Body { get; }
    }
}
