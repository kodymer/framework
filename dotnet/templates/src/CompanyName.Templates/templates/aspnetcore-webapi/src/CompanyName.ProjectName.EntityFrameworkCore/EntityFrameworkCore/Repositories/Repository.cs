using CompanyName.Ddd.Domain.Entities;
using CompanyName.EntityFrameworkCore.Repositories;

namespace CompanyName.ProjectName.EntityFrameworkCore.Repositories
{
    public abstract class Repository<TEntity, TKey> : EfCoreRepository<ProjectNameDbContext, TEntity>
        where TEntity : class, IAggregateRoot
    {

        protected Repository(ProjectNameDbContext dbContext)
            : base(dbContext)
        {
        }

        public virtual Task<TEntity> GetByIdAsync(IEntityId<TKey> id, CancellationToken cancellationToken = default)
        {
            return base.GetByIdAsync(id.Value, cancellationToken);
        }
    }

    public abstract class ProjectNameRepositoryBase<TEntity> : Repository<TEntity, int>
        where TEntity : class, IAggregateRoot
    {
        protected ProjectNameRepositoryBase(ProjectNameDbContext context)
            : base(context)
        {
        }
    }

}
