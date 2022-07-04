using Vesta.ServiceBus.Local;

namespace Vesta.EventBus
{
    public interface ILocalServiceBusMessageConsumer
    {
        void Initialize();

        void OnMessageReceived(Func<LocalServiceBusMessage, Task> processEventAsync);
    }
}