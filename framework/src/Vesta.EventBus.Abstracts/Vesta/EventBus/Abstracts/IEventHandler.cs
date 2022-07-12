namespace Vesta.EventBus.Abstracts
{
    public interface IEventHandler
    {

    }

    public interface IEventHandler<TArgs> : IEventHandler
        where TArgs : class
    {
        Task HandleEventAsync(TArgs args);
    }
}