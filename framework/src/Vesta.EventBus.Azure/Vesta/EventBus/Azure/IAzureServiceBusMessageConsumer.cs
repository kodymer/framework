using Azure.Messaging.ServiceBus;
using Vesta.ServiceBus.Azure;

namespace Vesta.EventBus.Azure
{
    public interface IAzureServiceBusMessageConsumer : IAsyncDisposable
    {
        void Initialize(string topicName, string subscriberName, string connectionString);

        void OnMessageReceived(Func<AzureServiceBusReceivedMessage, Task> processEventAsync);
    }
}