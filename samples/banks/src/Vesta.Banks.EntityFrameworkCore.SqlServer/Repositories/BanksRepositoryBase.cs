using Vesta.Ddd.Domain.Entities;
using Vesta.Domain.EntityFrameworkCore.Repositories;
using Vesta.EntityFrameworkCore;

namespace Vesta.Banks.EntityFrameworkCore.Repositories
{
    public abstract class BanksRepositoryBase<TEntity> : EfCoreRepository<BanksDbContext, TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected BanksRepositoryBase(IDbContextProvider<BanksDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }
    }

    public abstract class BanksRepositoryBase<TEntity, TKey> : EfCoreRepository<BanksDbContext, TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected BanksRepositoryBase(IDbContextProvider<BanksDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

    }

}
