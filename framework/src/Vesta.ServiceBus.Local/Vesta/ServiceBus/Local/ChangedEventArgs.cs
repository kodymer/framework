namespace Vesta.ServiceBus.Local
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
