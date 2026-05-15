namespace CompanyName.Ddd.Domain.Entities
{
    public interface IEntityId<out TKey>
    {
        public TKey Value { get; }
    }
}
