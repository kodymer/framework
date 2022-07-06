using Azure.Messaging.ServiceBus;

namespace Vesta.ServiceBus.Azure
{
    public interface IConnectionPool : IAsyncDisposable
    {
        ServiceBusClient GetClient(string connectionString);
    }
}

