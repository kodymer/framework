using Azure.Messaging.ServiceBus;

namespace Vesta.ServiceBus.Azure
{
    public interface IPublisherPool
    {

        ServiceBusSender GetSender(string connectionString, string topicName);
    }
}