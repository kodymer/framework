using CompanyName.Ddd.Domain.Entities;
using CompanyName.EntityFrameworkCore.Abstractions;
using CompanyName.EntityFrameworkCore.Repositories;

namespace CompanyName.Banks.EntityFrameworkCore.Repositories
{

    public abstract class Repository<TEntity> : EfCoreRepository<BanksDbContext, TEntity>
        where TEntity : class, IAggregateRoot
    {
        protected Repository(BanksDbContext dbContext)
            : base(dbContext)
        {

        }

    }
}
