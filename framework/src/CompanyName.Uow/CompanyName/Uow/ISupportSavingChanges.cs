namespace CompanyName.Uow
{
    public interface ISupportSavingChanges
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}