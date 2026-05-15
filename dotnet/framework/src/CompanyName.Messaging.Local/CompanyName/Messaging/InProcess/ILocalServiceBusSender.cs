namespace CompanyName.Messaging.InProcess
{
    public interface ILocalServiceBusSender
    {
        Task SendMessageAsync(LocalServiceBusMessage message);
    }
}