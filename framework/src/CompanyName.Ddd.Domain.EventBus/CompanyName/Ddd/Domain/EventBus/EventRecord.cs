using CommunityToolkit.Diagnostics;

namespace CompanyName.Ddd.Domain.EventBus
{
    public class EventRecord
    {
        public object Source { get; private set; }

        public object Data { get; private set; }

        public long Order { get; private set; }

        public EventRecord(object source, object data, long order)
        {
            Guard.IsNotNull(source);
            Guard.IsNotNull(data);
            Guard.IsGreaterThan(order, 0);

            Source = source;
            Data = data;
            Order = order;
        }
    }
}

