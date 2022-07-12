namespace Vesta.ServiceBus.Local
{
    public class ProcessMessageEventArgs : EventArgs{
    
        public LocalServiceBusMessage Message { get; }

        public ProcessMessageEventArgs(LocalServiceBusMessage message)
        {
            Message = message;
        }
    }
}
