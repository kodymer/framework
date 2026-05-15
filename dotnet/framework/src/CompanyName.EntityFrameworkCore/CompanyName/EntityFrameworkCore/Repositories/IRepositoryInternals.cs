using Ardalis.Specification;
using CompanyName.Ddd.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.EntityFrameworkCore.Repositories
{
    internal interface IRepositoryInternals<TEntity>
           where TEntity : class, IEntity
    {
        object MapperConfiguration { get; }

        IQueryable<TEntity> ApplySpecificationInternal(ISpecification<TEntity> specification);
        
        DbSet<TEntity> CreateDbSet();

    }
}
