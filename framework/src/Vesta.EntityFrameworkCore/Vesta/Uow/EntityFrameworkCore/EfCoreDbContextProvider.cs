using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading;
using Vesta.EntityFrameworkCore.Abstracts;

namespace Vesta.Uow.EntityFrameworkCore
{
    public class EfCoreDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : class, IEfCoreDbContext
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
