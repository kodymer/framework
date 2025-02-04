namespace CompanyName.EventBus.Abstracts
{
    public interface IEventBus
    {
        Task PublishAsync<TEvent>(TEvent eventData)
            where TEvent : class;
    }
}