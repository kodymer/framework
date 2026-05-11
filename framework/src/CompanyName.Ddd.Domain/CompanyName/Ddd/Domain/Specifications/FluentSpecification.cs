using System.Linq.Expressions;

namespace CompanyName.Ddd.Domain.Specifications
{
    internal class FluentSpecification<TEntity> : Specification<TEntity>
    {
        public FluentSpecification(Expression<Func<TEntity, bool>> criteria)
            : base(criteria)
        {
        }

        public new void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            base.AddInclude(includeExpression);
        }

        public new void ApplyOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            base.ApplyOrderBy(orderByExpression);
        }

        public new void ApplyOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        {
            base.ApplyOrderByDescending(orderByDescendingExpression);
        }

        public new void ApplyPaging(int skip, int take)
        {
            base.ApplyPaging(skip, take);
        }
    }
}


