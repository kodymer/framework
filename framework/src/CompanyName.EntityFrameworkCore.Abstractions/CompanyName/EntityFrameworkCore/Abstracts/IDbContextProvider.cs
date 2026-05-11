namespace CompanyName.EntityFrameworkCore.Abstractions
{
    public interface IDbContextProvider<TContext>
        where TContext : IExtendedDbContext
    {

        Task<TContext> GetDbContextAsync(CancellationToken cancellationToken = default);
    }
}
