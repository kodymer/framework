using Dapper;
using System.Data.Common;
using System.Reflection;
using System.Runtime.CompilerServices;
using Vesta.Dapper.Metadata;


[assembly: InternalsVisibleTo("Vesta.Dapper.SqlServer")]

namespace Vesta.Dapper
{
    public abstract class VestaDatabase<TDatabase> : Database<TDatabase> where TDatabase : Database<TDatabase>, new()
    {
        private IModelBuilder _modelBuilder;

        public VestaDatabase()
        {

        }

        new public Table<TEntity, TKey> Table<TEntity, TKey>()
            where TEntity : class
        {
            string likelyTableName;
            if (_modelBuilder.As<IModelBuilder>().EntityTypeBuilders.TryGetValue(typeof(TEntity), out var modelBuilder))
            {
                var entityTypeBuilder = new DapperEntityTypeBuilder<TEntity>();
                modelBuilder.As<Action<DapperEntityTypeBuilder<TEntity>>>().Invoke(entityTypeBuilder);

                if (entityTypeBuilder.As<IEntityTypeBuilder>().Tables.TryGetValue(typeof(TEntity), out likelyTableName))
                {
                    return new Table<TEntity, TKey>(this, likelyTableName);
                }
            };

            return new Table<TEntity, TKey>(this, typeof(TEntity).Name);
        }

        new public static TDatabase Init(DbConnection connection, int commandTimeout = DatabaseOptions.DefaultCommandTimeout)
        {
            var database = Database<TDatabase>.Init(connection, commandTimeout);

            var createModelBuilder = typeof(TDatabase).GetMethod(nameof(CreateModelBuilder), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            createModelBuilder.Invoke(database, null);

            return database;
        }

        internal virtual void CreateModelBuilder()
        {
            _modelBuilder = new DapperModelBuilder();
            OnModelCreating(_modelBuilder);
        }

        protected virtual void OnModelCreating(IModelBuilder modelBuilder)
        {

        }
    }
}
