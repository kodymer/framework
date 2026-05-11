using CompanyName.Core.Utilities.Expressions;
using System.Linq.Expressions;

namespace CompanyName.Ddd.Domain.Specifications
{
    public class OrSpecification<TEntity> : Specification<TEntity>
    {
        private readonly ISpecification<TEntity> _left;
        private readonly ISpecification<TEntity> _right;

        public OrSpecification(ISpecification<TEntity> left, ISpecification<TEntity> right)
        {
            _left = left ?? throw new ArgumentNullException(nameof(left));
            _right = right ?? throw new ArgumentNullException(nameof(right));

            Criteria = ExpressionCombiner.Or(_left.Criteria, _right.Criteria);

            // Combinar includes
            @base.IncludeExpressions.AddRange(_left.IncludeExpressions);
            @base.IncludeExpressions.AddRange(_right.IncludeExpressions.Where(include =>
                !@base.IncludeExpressions.Any(existing => existing.ToString() == include.ToString())));
        }
    }
}
