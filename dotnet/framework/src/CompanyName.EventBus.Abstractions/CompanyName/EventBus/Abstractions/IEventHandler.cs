namespace CompanyName.EventBus.Abstractions
{
    public interface IEventHandler
    {

    }

    public interface IEventHandler<TArgs> : IEventHandler
        where TArgs : class
    {
        Task HandleAsync(TArgs args, CancellationToken cancellationToken = default);
    }
}