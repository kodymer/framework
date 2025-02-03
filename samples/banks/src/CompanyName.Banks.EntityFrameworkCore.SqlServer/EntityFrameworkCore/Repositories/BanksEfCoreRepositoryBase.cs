using Vesta.Ddd.Domain.Entities;
using Vesta.Domain.EntityFrameworkCore.Repositories;
using Vesta.EntityFrameworkCore.Abstracts;

namespace Vesta.Banks.EntityFrameworkCore.Repositories
{

    public abstract class BanksEfCoreRepositoryBase<TEntity, TKey> : EfCoreRepository<BanksDbContext, TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected BanksEfCoreRepositoryBase(IDbContextProvider<BanksDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

    }

    public abstract class BanksEfCoreRepositoryBase<TEntity> : BanksEfCoreRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected BanksEfCoreRepositoryBase(IDbContextProvider<BanksDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }

}
