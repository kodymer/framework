namespace Vesta.ServiceBus.Local
{
    public class MessageSentEventArgs : EventArgs
    {
        public string Subject { get; }

        public string MessageId { get; }

        public object Body { get; }

        internal MessageSentEventArgs(LocalServiceBusMessage message)
        {
            Subject = message.Subject;
            MessageId = message.Subject;
            Body = message.Body;
        }
    }
}
