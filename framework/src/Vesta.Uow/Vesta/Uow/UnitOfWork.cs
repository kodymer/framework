using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Vesta.Core.DependencyInjection;
using Vesta.EventBus.Abstracts;

[assembly:
    InternalsVisibleTo("Vesta.EntityFrameworkCore"),
    InternalsVisibleTo("Vesta.Uow.Tests"),
    InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Vesta.Uow
{

    public class UnitOfWork : IUnitOfWork
    {
        public IServiceProvider ServiceProvider { get; }
        public event EventHandler Completed;
        public event EventHandler Completing;
        public event EventHandler<UnitOfWorkFailedEventArgs> Failed;
        public event EventHandler Disposed;
        public event EventHandler Rollbacked;

        public virtual bool IsCompleting { get; private set; }
        public virtual bool IsCompleted { get; private set; }
        public virtual bool IsReversed { get; private set; }
        public virtual IUnitOfWorkOptions Options { get; private set; }

        private IUnitOfWorkEventPublishingManager _eventPublishingManager;
        private IDictionary<string, IDatabaseApi> _databaseApis;
        private IDictionary<string, ITransactionApi> _transactionApis;
        private Exception _exception;
        private bool _isDisposed;

        private UnitOfWorkDefaultOptions _defaultOptions;

        public UnitOfWork(
            IServiceProvider serviceProvider,
            IUnitOfWorkEventPublishingManager eventPublishingManager,
            IOptions<UnitOfWorkDefaultOptions> options)
        {
            IsCompleting = false;
            IsCompleted = false;
            IsReversed = false;
            ServiceProvider = serviceProvider;
            Options = options.Value;

            _defaultOptions = options.Value;
            _databaseApis = new Dictionary<string, IDatabaseApi>();
            _transactionApis = new Dictionary<string, ITransactionApi>();

            _eventPublishingManager = eventPublishingManager;
        }

        internal void Initialize(UnitOfWorkOptions options)
        {
            Guard.Against.Null(options, nameof(options));

            if (Options is UnitOfWorkDefaultOptions defaultOptions)
            {
                Options = defaultOptions.Normalize(options);
            }
            else
            {
                throw new Exception("This unit of work is already initialized before!");
            }
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                foreach (var databaseApi in GetAllActiveDatabaseApis())
                {
                    if (databaseApi is ISupportSavingChanges api)
                    {
                        await api.SaveChangesAsync(cancellationToken);
                    }
                }
            }
            catch (Exception exception)
            {
                _exception = exception;

                throw;
            }
        }

        public async Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            if (IsReversed)
            {
                return;
            }

            PreventMultipleCompletions();

            try
            {

                IsCompleting = true;

                OnCompleting();

                await SaveChangesAsync(cancellationToken);

                await _eventPublishingManager.PublishAllAsync();

                await CommitTransactionAsync(cancellationToken);

                IsCompleted = true;

                OnCompleted();

            }
            catch (Exception exception)
            {
                _exception = exception;

                throw;
            }
        }

        private void PreventMultipleCompletions()
        {
            if (IsCompleting || IsCompleted)
            {
                throw new InvalidOperationException("Complete is called before!");
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (IsReversed)
            {
                return;
            }

            IsReversed = true;

            await RollbackAllAsync(cancellationToken);

            OnRollbacked();

            Options = _defaultOptions;

        }

        public void AddDatabaseApi(string key, IDatabaseApi api) => AddApi(_databaseApis, key, api);

        public IDatabaseApi FindDatabaseApi(string key) => FindApi(_databaseApis, key);

        public void AddTransactionApi(string key, ITransactionApi api) => AddApi(_transactionApis, key, api);

        public ITransactionApi FindTransactionApi(string key) => FindApi(_transactionApis, key);

        public async Task AddEventRecordAsync<TPublisher>(
            UnitOfWorkEventRecord unitOfWorkEventRecord,
            long priority,
            CancellationToken cancellationToken = default)
            where TPublisher : class, IEventBus
        {
            Guard.Against.Null(unitOfWorkEventRecord, nameof(unitOfWorkEventRecord));

            var publisher = ServiceProvider.GetRequiredService<TPublisher>();
            await _eventPublishingManager.CreateAndInsertAsync(publisher, unitOfWorkEventRecord, priority);
        }

        protected IReadOnlyList<IDatabaseApi> GetAllActiveDatabaseApis()
        {
            return _databaseApis.Values.ToImmutableList();
        }
        protected IReadOnlyList<ITransactionApi> GetAllActiveTransactionApis()
        {
            return _transactionApis.Values.ToImmutableList();
        }

        protected async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            foreach (var transaction in GetAllActiveTransactionApis())
            {
                await transaction.CommitAsync(cancellationToken);
            }
        }

        protected async Task RollbackAllAsync(CancellationToken cancellationToken = default)
        {
            foreach (var database in GetAllActiveDatabaseApis())
            {
                if (database is ISupportRollback)
                {
                    await database.As<ISupportRollback>().RollbackAsync(cancellationToken);
                }
            }

            foreach (var transaction in GetAllActiveTransactionApis())
            {
                if (transaction is ISupportRollback)
                {
                    await transaction.As<ISupportRollback>().RollbackAsync(cancellationToken);
                }
            }
        }

        protected T FindApi<T>(IDictionary<string, T> apis, string key)
            where T : class
        {
            Guard.Against.NullOrEmpty(key);

            if (apis.TryGetValue(key, out T api))
            {
                return api;
            }

            return null;
        }

        protected void AddApi<T>(IDictionary<string, T> apis, string key, T api)
            where T : class
        {
            Guard.Against.NullOrEmpty(key);
            Guard.Against.Null(api, nameof(api));

            if (apis.ContainsKey(key))
            {
                throw new InvalidOperationException("There is already a API in this unit of work with given key: " + key);
            }

            apis.Add(key, api);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // TODO: Managed objects
                }

                _databaseApis.Clear();

                DisposeTransactions();

                _isDisposed = true;

                if (!IsCompleted || _exception != null)
                {
                    OnFailed();
                }
            }

            OnDisposed();
        }

        private void DisposeTransactions()
        {
            foreach (var transactionApi in GetAllActiveTransactionApis())
            {
                try
                {
                    transactionApi.Dispose();
                }
                catch
                {
                }
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void OnCompleting()
        {
            Completing?.Invoke(this, EventArgs.Empty);
        }

        private void OnCompleted()
        {
            Completed?.Invoke(this, EventArgs.Empty);
        }

        private void OnFailed()
        {
            var args = new UnitOfWorkFailedEventArgs(_exception);
            Failed?.Invoke(this, args);
        }

        private void OnRollbacked()
        {
            Rollbacked?.Invoke(this, EventArgs.Empty);
        }

        private void OnDisposed()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
