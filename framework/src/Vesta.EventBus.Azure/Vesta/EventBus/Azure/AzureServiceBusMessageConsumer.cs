using Ardalis.GuardClauses;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Concurrent;
using System.Text.Json;
using Vesta.ServiceBus.Azure;

namespace Vesta.EventBus.Azure
{
    public class AzureServiceBusMessageConsumer : IAzureServiceBusMessageConsumer
    {
        public ILogger<AzureServiceBusMessageConsumer> Logger { get; set; }

        private string _topicName;
        private string _subscriberName;
        private string _connectionString;

        private readonly ConcurrentBag<Func<AzureServiceBusReceivedMessage, Task>> _callbacks;
        private readonly IProcessorPool _processorPool;

        public AzureServiceBusMessageConsumer(IProcessorPool processorPool)
        {
            Logger = NullLogger<AzureServiceBusMessageConsumer>.Instance;

            _callbacks = new ConcurrentBag<Func<AzureServiceBusReceivedMessage, Task>>();
            _processorPool = processorPool;
        }

        public void Initialize(string connectionString, string topicName, string subscriberName)
        {

            Guard.Against.NullOrEmpty(topicName, nameof(topicName));
            Guard.Against.NullOrEmpty(subscriberName, nameof(subscriberName));
            Guard.Against.NullOrEmpty(connectionString, nameof(connectionString));

            _topicName = topicName;
            _subscriberName = subscriberName;
            _connectionString = connectionString;

            StartProcessing();
        }

        public void OnMessageReceived(Func<AzureServiceBusReceivedMessage, Task> processEventAsync)
        {
            _callbacks.Add(processEventAsync);
        }

        protected virtual void StartProcessing()
        {
            Task.Factory.StartNew(function: async () =>
            {
                var serviceBusProcessor = _processorPool.GetProcessor(_connectionString, _topicName, _subscriberName);
                serviceBusProcessor.ProcessMessageAsync += MessageHandlerAsync;
                serviceBusProcessor.ProcessErrorAsync += ErrorHandlerAsync;

                if (!serviceBusProcessor.IsProcessing)
                {
                    await serviceBusProcessor.StartProcessingAsync();
                }

                while (true)
                {
                    Thread.Sleep(1000);
                }

            }, TaskCreationOptions.LongRunning);
        }

        protected async Task MessageHandlerAsync(ProcessMessageEventArgs args)
        {
            try
            {
                Logger.LogDebug($"Processing event type {args.Message.Subject}");

                foreach (var callbacks in _callbacks)
                {
                    Logger.LogDebug($"Executing callback.");

                    await callbacks(new AzureServiceBusReceivedMessage(args.Message));
                }

                Logger.LogInformation($"Message delivery: {JsonSerializer.Serialize(args.Message)}.");

                await args.CompleteMessageAsync(args.Message);

                Logger.LogDebug($"The message has already been completed.");
            }
            catch (Exception exception)
            {
                HandlerError(exception);
            }
        }

        protected Task ErrorHandlerAsync(ProcessErrorEventArgs args)
        {
            HandlerError(args.Exception);

            return Task.CompletedTask;
        }

        private void HandlerError(Exception exception)
        {

            Logger.LogError(exception, "There was a problem trying to process the message.");
        }
    }
}
