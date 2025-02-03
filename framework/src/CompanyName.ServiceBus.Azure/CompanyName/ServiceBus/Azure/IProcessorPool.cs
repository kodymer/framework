using Azure.Messaging.ServiceBus;

namespace CompanyName.ServiceBus.Azure
{
    public interface IProcessorPool : IAsyncDisposable
    {
        ServiceBusProcessor GetProcessor(string connectionString, string topicName, string subscriberName);
    }
}

