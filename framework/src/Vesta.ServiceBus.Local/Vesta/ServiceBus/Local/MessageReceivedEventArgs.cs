namespace Vesta.ServiceBus.Local
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public string Subject { get; }

        public string MessageId { get; }

        public object Body { get; }

        internal MessageReceivedEventArgs(LocalServiceBusMessage message)
        {
            Subject = message.Subject;
            MessageId = message.Subject;
            Body = message.Body;
        }
    }
}
