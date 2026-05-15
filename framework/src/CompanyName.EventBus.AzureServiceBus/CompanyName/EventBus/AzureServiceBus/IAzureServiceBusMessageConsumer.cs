using Azure.Messaging.ServiceBus;
using CompanyName.Messaging.AzureServiceBus;

namespace CompanyName.EventBus.AzureServiceBus
{
    public interface IAzureServiceBusMessageConsumer : IAsyncDisposable
    {
        void Initialize(string topicName, string subscriberName, string connectionString);

        void OnMessageReceived(Func<AzureServiceBusReceivedMessage, Task> processEventAsync);
    }
}