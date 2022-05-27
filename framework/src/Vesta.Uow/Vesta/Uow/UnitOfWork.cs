using Ardalis.GuardClauses;
using System.Collections.Immutable;
using Vesta.Core.DependencyInjection;

namespace Vesta.Uow
{
    public class UnitOfWork : IUnitOfWork, IServiceProviderAccessor
    {
        private IDictionary<string, IDatabaseApi> _databaseApis;

        public IServiceProvider ServiceProvider { get; }

        public UnitOfWork(IServiceProvider serviceProvider)
        {
            _databaseApis = new Dictionary<string, IDatabaseApi>();

            ServiceProvider = serviceProvider;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var databaseApi in GetDatabaseApis())
            {
                if (databaseApi is ISupportSavingChanges api)
                {
                    await api.SaveChangesAsync(cancellationToken);
                }
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
    }
}
