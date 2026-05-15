using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using AutoMapper;
using CompanyName.Ddd.Domain.Entities;
using CompanyName.Ddd.Domain.Repositories;
using CompanyName.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.EntityFrameworkCore.Repositories
{
    public class EfCoreReadOnlyRepository<TDbContext, TEntity> : RepositoryBase<TEntity>, IReadOnlyRepository<TEntity>, IRepositoryInternals<TEntity>
        where TEntity : class, IEntity
        where TDbContext : CompanyNameDbContextBase<TDbContext>
    {

        protected internal IConfigurationProvider ConfigurationProvider { get; set; }

        public object MapperConfiguration => ConfigurationProvider;

        public EfCoreReadOnlyRepository(TDbContext dbContext)
          : base(dbContext)
        {

        }

        public EfCoreReadOnlyRepository(TDbContext dbContext, ISpecificationEvaluator specificationEvaluator)
            : base(dbContext, specificationEvaluator)
        {
        }

        public IQueryable<TEntity> ApplySpecificationInternal(ISpecification<TEntity> specification) 
            => ApplySpecification(specification);

        public Task<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken = default)
            => DbContext.Set<TEntity>().FindAsync(keyValues, cancellationToken).AsTask();

        public DbSet<TEntity> CreateDbSet() 
            => DbContext.Set<TEntity>();
     
    }
}
