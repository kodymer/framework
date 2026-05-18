namespace CompanyName.Messaging.InProcess
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
