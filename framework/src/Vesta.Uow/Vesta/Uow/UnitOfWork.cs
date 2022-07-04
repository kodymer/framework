using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Vesta.Core.DependencyInjection;
using Vesta.EventBus.Abstracts;

[assembly: InternalsVisibleTo("Vesta.EntityFrameworkCore")]

namespace Vesta.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        public IServiceProvider ServiceProvider { get; }
        public event EventHandler Completed;
        public event EventHandler Completing;
        public event EventHandler Failed;
        public event EventHandler Disposing;
        public bool IsCompleting { get; private set; }
        public bool IsCompleted { get; private set; }

    private IUnitOfWorkEventPublishingManager _eventPublishingManager;
        private IDictionary<string, IDatabaseApi> _databaseApis;
        private Exception _exception;
        private bool _isDisposed;

        public UnitOfWork(
            IServiceProvider serviceProvider, 
            IUnitOfWorkEventPublishingManager eventPublishingManager)
        {
            IsCompleting = false;
            IsCompleted = false;
            ServiceProvider = serviceProvider;

            _databaseApis = new Dictionary<string, IDatabaseApi>();

            _eventPublishingManager = eventPublishingManager;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                foreach (var databaseApi in GetDatabaseApis())
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
            if(IsCompleting || IsCompleted)
            {
                throw new InvalidOperationException("Complete is called before!");
            }
         
            try
            {

                IsCompleting = true;

                OnCompleting();               

                await SaveChangesAsync(cancellationToken);

                await _eventPublishingManager.PublishAllAsync();

                IsCompleted = true;

                OnCompleted();

            }
            catch (Exception exception)
            {
                _exception = exception;

                throw;
            }
        }

        protected IReadOnlyList<IDatabaseApi> GetDatabaseApis()
        {
            return _databaseApis.Values.ToImmutableList();
        }

        public void AddDatabaseApi(string key, IDatabaseApi api)
        {
            Guard.Against.NullOrEmpty(key);
            Guard.Against.Null(api, nameof(api));

            if (_databaseApis.ContainsKey(key))
            {
                throw new InvalidOperationException("There is already a database API in this unit of work with given key: " + key);
            }

            _databaseApis.Add(key, api);
        }

        public IDatabaseApi FindDatabaseApi(string key)
        {
            Guard.Against.NullOrEmpty(key);

            if (_databaseApis.TryGetValue(key, out IDatabaseApi api))
            {
                return api;
            }

            return null;
        }

        public async Task AddEventRecordAsync<TPublisher>(
            UnitOfWorkEventRecord unitOfWorkEventRecord, 
            long priority, 
            CancellationToken cancellationToken = default)
            where TPublisher : class, IEventBus
        {
            Guard.Against.Null(unitOfWorkEventRecord, nameof(unitOfWorkEventRecord));

            var publisher = ServiceProvider.GetRequiredService<TPublisher>();
            var publishing = new UnitOfWorkEventPublishing(publisher, unitOfWorkEventRecord, priority);
            await _eventPublishingManager.CreateAsync(publishing);
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

                _isDisposed = true;

                if (!IsCompleted || _exception != null)
                {
                    OnFailed();
                }
            }

            OnDisposing();
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
        private void OnDisposing()
        {
            Disposing?.Invoke(this, EventArgs.Empty);
        }
    }
}
