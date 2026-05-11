using CommunityToolkit.Diagnostics;
using CompanyName.EntityFrameworkCore;
using CompanyName.EntityFrameworkCore.Abstractions;
using CompanyName.Uow.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CompanyName.Uow
{
    public class UnitOfWorkApiFactory<TContext> : IUnitOfWorkApiFactory<TContext>
        where TContext : IExtendedDbContext
    {
        public ILogger Logger { get; set; }

        public TContext DbContext
        {
            get
            {
                return _dbContext ??= UnitOfWork.ServiceProvider.GetRequiredService<TContext>();
            }
        }

        public IUnitOfWork UnitOfWork { get; }

        public UnitOfWorkApiFactory(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;

            Logger = NullLogger<UnitOfWorkApiFactory<TContext>>.Instance;
        }

        public IDatabaseApi GetDatabaseApi(CancellationToken cancellationToken = default)
        {
            var databaseApiKey = EfCoreDatabaseApi.GetKey(DbContext);
            var databaseApi = UnitOfWork.FindDatabaseApi(databaseApiKey) ??
                CreateDatabaseApi(databaseApiKey, cancellationToken);

            return databaseApi;
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
                                await DbContext.Database.BeginTransactionAsync(UnitOfWork.Options.IsolationLevel.Value);
                            }
                            else
                            {
                                await DbContext.Database.BeginTransactionAsync();
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

                        await DbContext.Database.BeginTransactionAsync();
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

        public IDatabaseApi CreateDatabaseApi(string key, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNullOrEmpty(key);

            DbContext.As<IInitializableDbContext>()?.Initialize(
                new DbContextInitializationContext(UnitOfWork));

            var databaseApi = new EfCoreDatabaseApi(DbContext);
            UnitOfWork.AddDatabaseApi(key, databaseApi);

            return databaseApi;
        }

        public Task<IDatabaseApi> CreateDatabaseApiAsync(string key, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNullOrEmpty(key);

            DbContext.As<IInitializableDbContext>()?.Initialize(
                new DbContextInitializationContext(UnitOfWork));

            var databaseApi = new EfCoreDatabaseApi(DbContext);
            UnitOfWork.AddDatabaseApi(key, databaseApi);

            return Task.FromResult<IDatabaseApi>(databaseApi);
        }

        public async Task<ITransactionApi> CreateTransactionApiAsync(string key, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNullOrEmpty(key);

            var dbContextTransaction = UnitOfWork.Options.IsolationLevel.HasValue ?
                await DbContext.Database.BeginTransactionAsync(UnitOfWork.Options.IsolationLevel.Value, cancellationToken) :
                await DbContext.Database.BeginTransactionAsync(cancellationToken);

            var transactionApi = new EfCoreTransactionApi(dbContextTransaction, DbContext);
            UnitOfWork.AddTransactionApi(key, transactionApi);

            return transactionApi;
        }


        private const string TransactionsNotSupportedErrorMessage = @"Current database does not support
                                                                      transactions. Your database may
                                                                      remain in an inconsistent state 
                                                                      in an error case.";

        private TContext _dbContext;
    }
}
