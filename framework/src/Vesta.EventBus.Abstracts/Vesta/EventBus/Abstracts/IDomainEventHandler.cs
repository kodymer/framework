using Vesta.EventBus.Abstracts;

namespace Vesta.EventBus.Abstracts
{
    public interface IDomainEventHandler<TArgs> : IEventHandler<TArgs>
        where TArgs : class
    {

    }
}
