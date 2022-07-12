namespace Vesta.ServiceBus.Local
{
    public interface ILocalServiceBusProcessor : IDisposable
    {
        bool IsProcessing { get; }

        event Func<ProcessMessageEventArgs, Task> ProcessMessageAsync;

        event Func<ProcessErrorEventArgs, Task> ProcessErrorAsync;

        Task StartProcessingAsync(CancellationToken cancellationToken = default);
    }
}