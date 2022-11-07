namespace Vesta.Uow
{
    public interface IUnitOfWorkApiFactory<TDbContext>
    {

        public IUnitOfWork UnitOfWork { get; }

        Task<IDatabaseApi> GetDatabaseApiAsync(CancellationToken cancellationToken = default);

        Task<ITransactionApi> GetTransactionApiAsync(CancellationToken cancellationToken = default);

        Task<IDatabaseApi> CreateDatabaseApiAsync(string key, CancellationToken cancellationToken = default);

        Task<ITransactionApi> CreateTransactionApiAsync(string key, CancellationToken cancellationToken = default);

    }
}
