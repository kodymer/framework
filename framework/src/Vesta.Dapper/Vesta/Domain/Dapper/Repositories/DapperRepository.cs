using Ardalis.GuardClauses;
using Vesta.Ddd.Domain.Entities;

namespace Vesta.Dapper.Domain.Repositories
{
    public abstract class DapperRepository<TDatabase, TEntity, TKey>
        where TDatabase : VestaDatabase<TDatabase>, new()
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

        public VestaDatabase<TDatabase>.Table<TEntity, TKey> GetTable()
        {
            return Database.Table<TEntity, TKey>();
        }

        
    }
}