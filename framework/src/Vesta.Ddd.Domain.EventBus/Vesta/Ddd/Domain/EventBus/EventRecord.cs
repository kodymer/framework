using Ardalis.GuardClauses;

namespace Vesta.Ddd.Domain.EventBus
{
    public class EventRecord
    {
        public object Source { get; private set; }

        public object Data { get; private set; }

        public long Order { get; private set; }

        public EventRecord(object source, object data, long order)
        {
            Guard.Against.Null(source, nameof(source));
            Guard.Against.Null(data, nameof(data));
            Guard.Against.NegativeOrZero(order, nameof(order));

            Source = source;
            Data = data;
            Order = order;
        }
    }
}

