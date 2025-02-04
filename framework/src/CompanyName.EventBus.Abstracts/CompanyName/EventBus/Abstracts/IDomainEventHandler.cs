using CompanyName.EventBus.Abstracts;

namespace CompanyName.EventBus.Abstracts
{
    public interface IDomainEventHandler<TArgs> : IEventHandler<TArgs>
        where TArgs : class
    {

    }
}
