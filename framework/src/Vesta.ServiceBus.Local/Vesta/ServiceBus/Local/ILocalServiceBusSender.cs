namespace Vesta.ServiceBus.Local
{
    public interface ILocalServiceBusSender
    {
        Task SendMessageAsync(LocalServiceBusMessage message);
    }
}