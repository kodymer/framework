namespace Vesta.Ddd.Domain.EventBus
{
    public class EventRecordOrderGenerator
    {
        private static long _currentValue;

        public static long GetNext()
        {
            return Interlocked.Increment(ref _currentValue);
        }
    }
}

