namespace Vesta.ServiceBus.Local
{
    public interface ILocalServiceBusProcessor
    {
        bool IsProcessing { get; }

        event Func<ProcessMessageEventArgs, Task> ProcessMessageAsync;

        event Func<ProcessErrorEventArgs, Task> ProcessErrorAsync;

        Task StartProcessingAsync();
    }
}