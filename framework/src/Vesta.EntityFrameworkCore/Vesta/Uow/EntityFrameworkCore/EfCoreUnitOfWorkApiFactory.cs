using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Vesta.EntityFrameworkCore;
using Vesta.EntityFrameworkCore.Abstracts;

namespace Vesta.Uow.EntityFrameworkCore
{
    public class EfCoreUnitOfWorkApiFactory<TDbContext> : IUnitOfWorkApiFactory<TDbContext>
        where TDbContext : IEfCoreDbContext
    {

        private const string TransactionsNotSupportedErrorMessage = @"Current database does not support
                                                                      transactions. Your database may
                                                                      remain in an inconsistent state 
                                                                      in an error case.";
        public ILogger Logger { get; set; }

        public TDbContext DbContext
        {
            get
            {
                return _dbContext ??= UnitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
            }
        }

        public IUnitOfWork UnitOfWork { get; }

        private TDbContext _dbContext;


        public EfCoreUnitOfWorkApiFactory(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;

            Logger = NullLogger<EfCoreUnitOfWorkApiFactory<TDbContext>>.Instance;
        }

        public async Task<IDatabaseApi> GetDatabaseApiAsync(CancellationToken cancellationToken = default)
        {
            var databaseApiKey = EfCoreDatabaseApi.GetKey(DbContext);
            var databaseApi = UnitOfWork.FindDatabaseApi(databaseApiKey) ?? 
                await CreateDatabaseApiAsync(databaseApiKey, cancellationToken);

            return databaseApi;
        }

        public async Task<ITransactionApi> GetTransactionApiAsync(CancellationToken cancellationToken = default)
        {
            var transactionApiKey = EfCoreTransactionApi.GetKey(DbContext);
            var transactionApi = UnitOfWork.FindTransactionApi(transactionApiKey) as EfCoreTransactionApi;
            if (transactionApi is null)
            {
                transactionApi = await CreateTransactionApiAsync(transactionApiKey, cancellationToken) as EfCoreTransactionApi;
            }
            else
            {

                /*
                 * You can also share a transaction across multiple context instances. This functionality 
                 * is only available when using a relational database provider because it requires the use 
                 * of DbTransaction and DbConnection, which are specific to relational databases. 
                 * 
                 * To share a transaction, the contexts must share both a DbConnection and a DbTransaction.
                 *
                 * See https://learn.microsoft.com/es-es/ef/core/saving/transactions#cross-context-transaction
                 * 
                 */

                if (DbContext.As<DbContext>().HasRelationalTransactionManager())
                {

                    if (DbContext.Database.GetDbConnection() == transactionApi.DbContextTransaction.GetDbTransaction().Connection)
                    {
                        await DbContext.Database.UseTransactionAsync(transactionApi.DbContextTransaction.GetDbTransaction());
                    }
                    else
                    {

                        try
                        {
                            /* 
                             * User did not re-use the ExistingConnection and we are starting a new transaction.
                             * EfCoreTransactionApi will check the connection string match and separately
                             * commit/rollback this transaction over the DbContext instance. 
                             */

                            if (UnitOfWork.Options.IsolationLevel.HasValue)
                            {
                                DbContext.Database.BeginTransaction(UnitOfWork.Options.IsolationLevel.Value);
                            }
                            else
                            {
                                DbContext.Database.BeginTransaction();
                            }
                        }
                        catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
                        {
                            Logger.LogError(e, TransactionsNotSupportedErrorMessage);

                            return transactionApi;
                        }
                    }
                }
                else
                {
                    try
                    {
                        /* 
                         * No need to store the returning IDbContextTransaction for non-relational databases
                         * since EfCoreTransactionApi will handle the commit/rollback over the DbContext instance.
                         */

                        DbContext.Database.BeginTransaction();
                    }
                    catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
                    {
                        Logger.LogError(e, TransactionsNotSupportedErrorMessage);

                        return transactionApi;
                    }
                }

                transactionApi.AttendedDbContexts.Add(DbContext);
            }

            return transactionApi;
        }

        public Task<IDatabaseApi> CreateDatabaseApiAsync(string key, CancellationToken cancellationToken = default)
        {
            Guard.Against.NullOrEmpty(key, nameof(key));

            DbContext.As<IStartableEfCoreDbContext>()?.Initialize(
                new EfCoreDbContextInitianlizationContext(UnitOfWork));

            var databaseApi = new EfCoreDatabaseApi(DbContext);
            UnitOfWork.AddDatabaseApi(key, databaseApi);

            return Task.FromResult<IDatabaseApi>(databaseApi);
        }

        public async Task<ITransactionApi> CreateTransactionApiAsync(string key, CancellationToken cancellationToken = default)
        {
            Guard.Against.NullOrEmpty(key, nameof(key));

            var dbContextTransaction = UnitOfWork.Options.IsolationLevel.HasValue ?
                await DbContext.Database.BeginTransactionAsync(UnitOfWork.Options.IsolationLevel.Value, cancellationToken) :
                await DbContext.Database.BeginTransactionAsync(cancellationToken);

            var transactionApi = new EfCoreTransactionApi(dbContextTransaction, DbContext);
            UnitOfWork.AddTransactionApi(key, transactionApi);

            return transactionApi;
        }
    }
}
