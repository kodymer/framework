namespace Vesta.Uow
{
    public interface ISupportSavingChanges
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}