namespace Vesta.Dapper.Metadata
{
    public interface IEntityTypeBuilder
    {
        Dictionary<Type, string> Tables { get; }
    }
}