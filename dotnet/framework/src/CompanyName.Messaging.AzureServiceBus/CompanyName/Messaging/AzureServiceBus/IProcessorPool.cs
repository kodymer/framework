using Azure.Messaging.ServiceBus;

namespace CompanyName.Messaging.AzureServiceBus
{
    public interface IProcessorPool : IAsyncDisposable
    {
        ServiceBusProcessor GetProcessor(string connectionString, string topicName, string subscriberName);
    }
}

