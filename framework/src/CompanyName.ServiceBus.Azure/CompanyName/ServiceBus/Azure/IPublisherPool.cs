using Azure.Messaging.ServiceBus;

namespace CompanyName.ServiceBus.Azure
{
    public interface IPublisherPool : IAsyncDisposable
    {

        ServiceBusSender GetSender(string connectionString, string topicName);
    }
}