using CompanyName.Ddd.Domain.Entities;
using CompanyName.EntityFrameworkCore.Repositories;

namespace CompanyName.ProjectName.EntityFrameworkCore.Repositories
{
    public abstract class ReadOnlyRepository<TEntity, TKey> : EfCoreReadOnlyRepository<ProjectNameDbContext, TEntity>
        where TEntity : class, IEntity
    {

        protected ReadOnlyRepository(ProjectNameDbContext dbContext)
            : base(dbContext)
        {
        }

        public virtual Task<TEntity> GetByIdAsync(IEntityId<TKey> id, CancellationToken cancellationToken = default)
        {
            return base.GetByIdAsync(id.Value, cancellationToken);
        }
    }


    public abstract class ReadOnlyRepository<TEntity> : ReadOnlyRepository<TEntity, int>
        where TEntity : class, IEntity
    {
        protected ReadOnlyRepository(ProjectNameDbContext context)
            : base(context)
        {
        }
    }
}
