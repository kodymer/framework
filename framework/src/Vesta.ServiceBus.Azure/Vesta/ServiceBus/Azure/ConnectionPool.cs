using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;

namespace Vesta.ServiceBus.Azure
{

    public class ConnectionPool : PoolBase<ServiceBusClient>, IConnectionPool
        
    {
        private readonly ServiceBusClientOptions _options;

        public ConnectionPool(IOptions<ServiceBusClientOptions> options)
        {
            _options = options.Value;
        }

        public ServiceBusClient GetClient(string connectionString)
        {
            return GetOrAdd(
                connectionString, 
                new Lazy<ServiceBusClient>(() => new ServiceBusClient(connectionString, _options))).Value;
        }
    }
}

