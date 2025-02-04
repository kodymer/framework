using CompanyName.EventBus.Abstracts;

namespace CompanyName.EventBus.Abstracts
{
    public interface IIntegrationEventHandler<TArgs> : IEventHandler<TArgs>
        where TArgs : class
    {

    }
}
