namespace CompanyName.EventBus.Abstractions
{
    public interface IEventBus
    {
        Task PublishAsync<TEvent>(TEvent eventData)
            where TEvent : class;
    }
}