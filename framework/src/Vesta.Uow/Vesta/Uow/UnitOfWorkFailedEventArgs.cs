namespace Vesta.Uow
{
    public class UnitOfWorkFailedEventArgs : EventArgs
    {
        public UnitOfWorkFailedEventArgs(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; }
    }
}