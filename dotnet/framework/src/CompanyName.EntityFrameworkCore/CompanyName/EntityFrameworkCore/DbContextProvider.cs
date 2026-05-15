using CompanyName.EntityFrameworkCore.Abstractions;
using CompanyName.Uow;
using CompanyName.Uow.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Nito.AsyncEx;

namespace CompanyName.EntityFrameworkCore
{
    public class DbContextProvider<TContext> : IDbContextProvider<TContext>
        where TContext : CompanyNameDbContextBase<TContext>
    {

        private readonly IUnitOfWorkApiFactory<TContext> _unitOfWorkApiFactory;

        public ILogger Logger { get; set; }

        public DbContextProvider(IUnitOfWorkApiFactory<TContext> unitOfWorkApiFactory)
        {
            _unitOfWorkApiFactory = unitOfWorkApiFactory;

            Logger = NullLogger<DbContextProvider<TContext>>.Instance;
        }

        public Task<TContext> GetDbContextAsync(CancellationToken cancellationToken = default)
        {
            return CreateDbContextAsync();
        }

        private async Task<TContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
        {
            var databaseApi = await _unitOfWorkApiFactory.GetDatabaseApiAsync();

            if (_unitOfWorkApiFactory.UnitOfWork.Options.IsTransactional)
            {
                await _unitOfWorkApiFactory.GetTransactionApiAsync(cancellationToken);
            }

            return (TContext)((EfCoreDatabaseApi)databaseApi).DbContext;
        }
    }

    public class DbContextFactory<TContext> : IDbContextFactory<TContext>
        where TContext : CompanyNameDbContextBase<TContext>
    {
        private readonly IUnitOfWorkApiFactory<TContext> _unitOfWorkApiFactory;

        public ILogger Logger { get; set; }

        public DbContextFactory(IUnitOfWorkApiFactory<TContext> unitOfWorkApiFactory)
        {
            _unitOfWorkApiFactory = unitOfWorkApiFactory;

            Logger = NullLogger<DbContextProvider<TContext>>.Instance;
        }

        public TContext CreateDbContext()
        {
            return AsyncContext.Run(async () => await CreateDbContextAsync());
        }

        public async Task<TContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
        {
            var databaseApi = await _unitOfWorkApiFactory.GetDatabaseApiAsync();

            if (_unitOfWorkApiFactory.UnitOfWork.Options.IsTransactional)
            {
                await _unitOfWorkApiFactory.GetTransactionApiAsync(cancellationToken);
            }

            return (TContext)((EfCoreDatabaseApi)databaseApi).DbContext;
        }
    }
}
