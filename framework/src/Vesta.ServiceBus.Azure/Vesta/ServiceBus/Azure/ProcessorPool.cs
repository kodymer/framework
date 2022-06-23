using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Vesta.ServiceBus.Azure
{
    public class ProcessorPool : PoolBase<ServiceBusProcessor>, IProcessorPool

    {
        private readonly ServiceBusProcessorOptions _options;
        private readonly IConnectionPool _connectionPool;

        public ProcessorPool(
            IOptions<ServiceBusProcessorOptions> options,
            IConnectionPool connectionPool)
        {
            _options = options.Value;
            _connectionPool = connectionPool;
        }

        public ServiceBusProcessor GetProcessor(string connectionString, string topicName, string subscriberName)
        {
            return GetOrAdd(
                $"{topicName}_{subscriberName}", new Lazy<ServiceBusProcessor>(() =>
                {
                    var client = _connectionPool.GetClient(connectionString);
                    return client.CreateProcessor(topicName, subscriberName, _options);
                })
            ).Value;
        }

        protected override async Task DisposeAsync(ServiceBusProcessor processor)
        {
            if (processor.IsProcessing)
            {
                await processor.StopProcessingAsync();
            }

            if (!processor.IsClosed)
            {
                await processor.CloseAsync();
            }

            await base.DisposeAsync(processor);
        }
    }
}

