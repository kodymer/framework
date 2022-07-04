using Vesta.EventBus.Abstracts;

namespace Vesta.EventBus.Abstracts
{
    public interface IIntegrationEventHandler<TArgs> : IEventHandler<TArgs>
        where TArgs : class
    {

    }
}
