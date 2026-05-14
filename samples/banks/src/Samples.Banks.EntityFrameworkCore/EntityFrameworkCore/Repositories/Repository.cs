using CompanyName.Ddd.Domain.Entities;
using CompanyName.EntityFrameworkCore.Repositories;

namespace Samples.Banks.EntityFrameworkCore.Repositories
{
    public abstract class Repository<TEntity, TKey> : EfCoreRepository<BankDbContext, TEntity>
        where TEntity : class, IEntity<IEntityId<TKey>>, IAggregateRoot
    {

        protected Repository(BankDbContext dbContext)
            : base(dbContext)
        {
        }

        public virtual Task<TEntity> GetByIdAsync(IEntityId<TKey> id, CancellationToken cancellationToken = default)
        {
            return base.GetByIdAsync(id.Value, cancellationToken);
        }
    }

    public abstract class Repository<TEntity> : Repository<TEntity, int>
        where TEntity : class, IEntity<IEntityId<int>>, IAggregateRoot
    {
        protected Repository(BankDbContext context)
            : base(context)
        {
        }
    }
}
