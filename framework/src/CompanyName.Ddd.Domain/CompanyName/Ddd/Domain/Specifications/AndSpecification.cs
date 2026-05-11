using CompanyName.Core.Utilities.Expressions;

namespace CompanyName.Ddd.Domain.Specifications
{
    public class AndSpecification<TEntity> : Specification<TEntity>
    {
        private readonly ISpecification<TEntity> _left;
        private readonly ISpecification<TEntity> _right;

        public AndSpecification(ISpecification<TEntity> left, ISpecification<TEntity> right)
        {
            _left = left ?? throw new ArgumentNullException(nameof(left));
            _right = right ?? throw new ArgumentNullException(nameof(right));

            Criteria = ExpressionCombiner.And(_left.Criteria, _right.Criteria);

            @base.IncludeExpressions.AddRange(_left.IncludeExpressions);
            @base.IncludeExpressions.AddRange(_right.IncludeExpressions.Where(include =>
                !@base.IncludeExpressions.Any(existing => existing.ToString() == include.ToString())));
        }
    }
}
