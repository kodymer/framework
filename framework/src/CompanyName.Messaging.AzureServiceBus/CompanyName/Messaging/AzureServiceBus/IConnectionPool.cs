using Azure.Messaging.ServiceBus;

namespace CompanyName.Messaging.AzureServiceBus
{
    public interface IConnectionPool : IAsyncDisposable
    {
        ServiceBusClient GetClient(string connectionString);
    }
}

