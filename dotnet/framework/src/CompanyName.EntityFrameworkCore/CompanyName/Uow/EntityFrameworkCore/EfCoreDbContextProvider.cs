using CompanyName.EntityFrameworkCore.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CompanyName.Uow.EntityFrameworkCore
{
    public class EfCoreDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : class, IExtendedDbContext
    {

        private readonly IUnitOfWorkApiFactory<TDbContext> _unitOfWorkApiFactory;

        public ILogger Logger { get; set; }


        public EfCoreDbContextProvider(IUnitOfWorkApiFactory<TDbContext> unitOfWorkApiFactory)
        {
            _unitOfWorkApiFactory = unitOfWorkApiFactory;

            Logger = NullLogger<EfCoreDbContextProvider<TDbContext>>.Instance;
        }

        public Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
        {
            return CreateDbContextAsync();
        }

        private async Task<TDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
        {
            var databaseApi = await _unitOfWorkApiFactory.GetDatabaseApiAsync();

            if (_unitOfWorkApiFactory.UnitOfWork.Options.IsTransactional)
            {
                await _unitOfWorkApiFactory.GetTransactionApiAsync(cancellationToken);
            }

            return (TDbContext)((EfCoreDatabaseApi)databaseApi).DbContext;
        }

    }
}
