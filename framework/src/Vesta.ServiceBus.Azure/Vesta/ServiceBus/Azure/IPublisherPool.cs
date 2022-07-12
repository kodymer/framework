using Azure.Messaging.ServiceBus;

namespace Vesta.ServiceBus.Azure
{
    public interface IPublisherPool : IAsyncDisposable
    {

        ServiceBusSender GetSender(string connectionString, string topicName);
    }
}