using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.ServiceBus.Azure
{
    public class PublisherPool : PoolBase<ServiceBusSender>, IPublisherPool
    {
        private readonly IConnectionPool _connectionPool;

        public PublisherPool(
            IConnectionPool connectionPool)
        {
            _connectionPool = connectionPool;
        }

        public ServiceBusSender GetSender(string connectionString, string topicName)
        {
            return GetOrAdd(
                $"{topicName}", new Lazy<ServiceBusSender>(() =>
                {
                    var client = _connectionPool.GetClient(connectionString);
                    return client.CreateSender(topicName);
                })
            ).Value;
        }

        protected override Task DisposeAsync(ServiceBusSender element)
        {
            element.CloseAsync();   

            return base.DisposeAsync(element);
        }
    }
}
