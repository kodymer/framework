using System.Linq.Expressions;

namespace CompanyName.Ddd.Domain.Specifications.Factories
{
    public class SpecificationFactory<TEntity> : ISpecificationFactory<TEntity>
    {
        public ISpecification<TEntity> Create(Expression<Func<TEntity, bool>> criteria)
        {
            return new AdHocSpecification<TEntity>(criteria);
        }

        public ISpecificationBuilder<TEntity> Builder()
        {
            return new SpecificationBuilder<TEntity>();
        }

        public ISpecification<TEntity> True()
        {
            return new AdHocSpecification<TEntity>(x => true);
        }

        public ISpecification<TEntity> False()
        {
            return new AdHocSpecification<TEntity>(x => false);
        }
    }
}


