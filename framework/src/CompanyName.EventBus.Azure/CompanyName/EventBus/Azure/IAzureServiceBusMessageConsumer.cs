using Azure.Messaging.ServiceBus;
using CompanyName.ServiceBus.Azure;

namespace CompanyName.EventBus.Azure
{
    public interface IAzureServiceBusMessageConsumer : IAsyncDisposable
    {
        void Initialize(string topicName, string subscriberName, string connectionString);

        void OnMessageReceived(Func<AzureServiceBusReceivedMessage, Task> processEventAsync);
    }
}