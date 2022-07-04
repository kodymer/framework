using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Concurrent;
using System.Text.Json;
using Vesta.ServiceBus.Local;

namespace Vesta.EventBus.Azure
{
    public class LocalServiceBusMessageConsumer : ILocalServiceBusMessageConsumer
    {
        public ILogger<LocalServiceBusMessageConsumer> Logger { get; set; }

        private readonly ConcurrentBag<Func<LocalServiceBusMessage, Task>> _callbacks;
        private readonly ILocalServiceBusProcessor _processor;

        public LocalServiceBusMessageConsumer(ILocalServiceBusProcessor processor)
        {
            Logger = NullLogger<LocalServiceBusMessageConsumer>.Instance;

            _callbacks = new ConcurrentBag<Func<LocalServiceBusMessage, Task>>();
            _processor = processor;
        }

        public void Initialize()
        {
            StartProcessing();
        }

        public void OnMessageReceived(Func<LocalServiceBusMessage, Task> processEventAsync)
        {
            _callbacks.Add(processEventAsync);
        }

        protected virtual void StartProcessing()
        {
            Task.Factory.StartNew(function: async () =>
            {
                _processor.ProcessMessageAsync += MessageHandlerAsync;
                _processor.ProcessErrorAsync += ErrorHandlerAsync;

                if (!_processor.IsProcessing)
                {
                    await _processor.StartProcessingAsync();
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

                    await callbacks(args.Message);
                }

                Logger.LogInformation($"Message delivery: {JsonSerializer.Serialize(args.Message)}.");

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
