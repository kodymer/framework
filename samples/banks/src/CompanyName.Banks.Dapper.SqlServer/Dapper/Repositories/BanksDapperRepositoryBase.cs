using CompanyName.Dapper.Domain.Repositories;
using CompanyName.Ddd.Domain.Entities;

namespace CompanyName.Banks.Dapper.Repositories
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
