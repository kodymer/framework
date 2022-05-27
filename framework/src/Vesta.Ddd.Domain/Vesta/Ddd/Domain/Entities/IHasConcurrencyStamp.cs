namespace Vesta.Ddd.Domain.Entities
{
    public interface IHasConcurrencyStamp
    {
        string ConcurrencyStamp { get; set; }
    }
}