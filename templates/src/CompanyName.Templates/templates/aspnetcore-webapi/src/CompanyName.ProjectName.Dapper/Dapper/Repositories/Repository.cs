using CompanyName.Dapper.Repositories;
using CompanyName.Ddd.Domain.Entities;

namespace CompanyName.ProjectName.Dapper.Repositories
{
    public abstract class Repository<TEntity, TKey> : DapperRepository<ProjectNameDatabase, TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected Repository(ProjectNameDatabase database)
            : base(database)
        {

        }
    }

    public abstract class Repository<TEntity> : Repository<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected Repository(ProjectNameDatabase database)
            : base(database)
        {

        }
    }

}
