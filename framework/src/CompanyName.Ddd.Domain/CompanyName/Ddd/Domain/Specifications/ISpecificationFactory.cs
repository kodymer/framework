using System.Linq.Expressions;

namespace CompanyName.Ddd.Domain.Specifications
{
    public interface ISpecificationFactory<TEntity>
    {
        ISpecification<TEntity> Create(Expression<Func<TEntity, bool>> criteria);
        ISpecificationBuilder<TEntity> Builder();
        ISpecification<TEntity> True();
        ISpecification<TEntity> False();
    }
}


