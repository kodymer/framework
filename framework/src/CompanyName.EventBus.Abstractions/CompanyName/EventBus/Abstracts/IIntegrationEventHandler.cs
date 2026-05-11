using CompanyName.EventBus.Abstractions;

namespace CompanyName.EventBus.Abstractions
{
    public interface IIntegrationEventHandler<TArgs> : IEventHandler<TArgs>
        where TArgs : class
    {

    }
}
