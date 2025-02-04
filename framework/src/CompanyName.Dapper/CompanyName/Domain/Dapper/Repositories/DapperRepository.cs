using Ardalis.GuardClauses;
using CompanyName.Ddd.Domain.Entities;

namespace CompanyName.Dapper.Domain.Repositories
{
    public abstract class DapperRepository<TDatabase, TEntity, TKey>
        where TDatabase : CompanyNameDatabase<TDatabase>, new()
        where TEntity : class, IEntity<TKey>
    {

        protected TDatabase Database { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="database">Database</param>
        public DapperRepository(TDatabase database)
        {
            Guard.Against.Null(database, nameof(database));

            Database = database;
        }

        public CompanyNameDatabase<TDatabase>.Table<TEntity, TKey> GetTable()
        {
            return Database.Table<TEntity, TKey>();
        }

        
    }
}