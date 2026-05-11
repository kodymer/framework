namespace CompanyName.Ddd.Domain.Entities
{
    public record class EntityId<TKey>(TKey Value) : IEntityId<TKey>;
}
