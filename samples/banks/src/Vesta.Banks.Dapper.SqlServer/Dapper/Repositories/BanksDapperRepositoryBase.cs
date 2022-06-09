using Vesta.Dapper.Domain.Repositories;
using Vesta.Ddd.Domain.Entities;

namespace Vesta.Banks.Dapper.Repositories
{
    public abstract class BanksDapperRepositoryBase<TEntity, TKey> : DapperRepository<BanksDatabase, TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected BanksDapperRepositoryBase(BanksDatabase database)
            : base(database)
        {

        }
    }

    public abstract class BanksDapperRepositoryBase<TEntity> : BanksDapperRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected BanksDapperRepositoryBase(BanksDatabase database)
            : base(database)
        {

        }
    }

}
