using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.ServiceBus.Local
{
    public class LocalServiceBusProcessor : ILocalServiceBusProcessor
    {
        public bool IsProcessing { get; set; }

        private readonly LocalServiceBusQueue _queue;

        public event Func<ProcessMessageEventArgs, Task> ProcessMessageAsync;
        public event Func<ProcessErrorEventArgs, Task> ProcessErrorAsync;

        public LocalServiceBusProcessor(LocalServiceBusQueue queue)
        {
            _queue = queue;
        }

        public async Task StartProcessingAsync()
        {
            IsProcessing = true;

            await Task.Factory.StartNew(function: async () =>
            {
                while (true)
                {
                    if (_queue.TryDequeue(out var message))
                    {

                        try
                        {
                            await OnProcessMessage(message);
                        }
                        catch (Exception exception)
                        {
                            await OnProcessError(exception);
                        }
                    }

                    Thread.Sleep(1000);
                }
            }, TaskCreationOptions.LongRunning);
        }


        private async Task OnProcessMessage(LocalServiceBusMessage message)
        {
            if (ProcessErrorAsync is not null)
            {
                var args = new ProcessMessageEventArgs(message);
                await ProcessMessageAsync(args);
            }
        }
        private async Task OnProcessError(Exception exception)
        {
            if (ProcessErrorAsync is not null)
            {
                var args = new ProcessErrorEventArgs(exception);
                await ProcessErrorAsync(args);
            }
        }
    }
}
