using Azure.Messaging.ServiceBus;

namespace Vesta.ServiceBus.Azure
{
    public interface IProcessorPool
    {
        ServiceBusProcessor GetProcessor(string connectionString, string topicName, string subscriberName);
    }
}

