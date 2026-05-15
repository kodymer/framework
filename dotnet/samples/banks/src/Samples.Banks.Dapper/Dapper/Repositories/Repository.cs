using CompanyName.Dapper.Repositories;
using CompanyName.Ddd.Domain.Entities;

namespace Samples.Banks.Dapper.Repositories
{
    public abstract class Repository<TEntity, TKey> : DapperRepository<BanksDatabase, TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected Repository(BanksDatabase database)
            : base(database)
        {

        }
    }

    public abstract class Repository<TEntity> : Repository<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected Repository(BanksDatabase database)
            : base(database)
        {

        }
    }

}
