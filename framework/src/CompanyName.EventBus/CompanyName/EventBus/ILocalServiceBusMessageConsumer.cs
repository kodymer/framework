using CompanyName.ServiceBus.Local;

namespace CompanyName.EventBus
{
    public interface ILocalServiceBusMessageConsumer : IDisposable
    {
        void Initialize();

        void OnMessageReceived(Func<LocalServiceBusMessage, Task> processEventAsync);
    }
}