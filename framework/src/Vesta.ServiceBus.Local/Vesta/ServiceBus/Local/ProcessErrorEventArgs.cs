namespace Vesta.ServiceBus.Local
{
    public class ProcessErrorEventArgs : EventArgs
    {

        public Exception Exception { get; }

        public ProcessErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
