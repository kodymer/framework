using System.Linq.Expressions;

namespace CompanyName.Ddd.Domain.Specifications
{
    public interface ISpecificationBuilder<TEntity>
    {
        ISpecificationBuilder<TEntity> Where(Expression<Func<TEntity, bool>> criteria);
        
        ISpecificationBuilder<TEntity> And(Expression<Func<TEntity, bool>> criteria);
        
        ISpecificationBuilder<TEntity> Or(Expression<Func<TEntity, bool>> criteria);
        
        ISpecificationBuilder<TEntity> Not(Expression<Func<TEntity, bool>> criteria);
        
        ISpecificationBuilder<TEntity> Include(Expression<Func<TEntity, object>> includeExpression);
        
        ISpecificationBuilder<TEntity> OrderBy(Expression<Func<TEntity, object>> orderExpression);
        
        ISpecificationBuilder<TEntity> OrderByDescending(Expression<Func<TEntity, object>> orderExpression);
        
        ISpecificationBuilder<TEntity> Skip(int skip);
        
        ISpecificationBuilder<TEntity> Take(int take);
        
        ISpecificationBuilder<TEntity> Paginate(int page, int pageSize);
        
        ISpecification<TEntity> Build();
    }
}


