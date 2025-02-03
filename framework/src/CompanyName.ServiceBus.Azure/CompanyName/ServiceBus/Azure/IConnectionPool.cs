using Azure.Messaging.ServiceBus;

namespace CompanyName.ServiceBus.Azure
{
    public interface IConnectionPool : IAsyncDisposable
    {
        ServiceBusClient GetClient(string connectionString);
    }
}

