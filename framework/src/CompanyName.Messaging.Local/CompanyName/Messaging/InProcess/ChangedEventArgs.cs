namespace CompanyName.Messaging.InProcess
{
    public class ChangedEventArgs : EventArgs
    {
        public int OldTotalMessajeNumber { get; }

        public int CurrentTotalMessajeNumber { get; }

        internal ChangedEventArgs(int oldTotalMessajeNumber, int currentTotalMessajeNumber)
        {
            OldTotalMessajeNumber = oldTotalMessajeNumber;
            CurrentTotalMessajeNumber = currentTotalMessajeNumber;
        }
    }
}
