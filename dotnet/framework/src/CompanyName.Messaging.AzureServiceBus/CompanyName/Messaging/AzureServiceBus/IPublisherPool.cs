using Azure.Messaging.ServiceBus;

namespace CompanyName.Messaging.AzureServiceBus
{
    public interface IPublisherPool : IAsyncDisposable
    {

        ServiceBusSender GetSender(string connectionString, string topicName);
    }
}