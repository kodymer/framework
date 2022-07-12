using Vesta.ServiceBus.Local;

namespace Vesta.EventBus
{
    public interface ILocalServiceBusMessageConsumer : IDisposable
    {
        void Initialize();

        void OnMessageReceived(Func<LocalServiceBusMessage, Task> processEventAsync);
    }
}