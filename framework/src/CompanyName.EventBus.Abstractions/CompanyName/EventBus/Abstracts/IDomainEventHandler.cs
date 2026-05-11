using CompanyName.EventBus.Abstractions;

namespace CompanyName.EventBus.Abstractions
{
    public interface IDomainEventHandler<TArgs> : IEventHandler<TArgs>
        where TArgs : class
    {

    }
}
