using CompanyName.Core.Utilities.Expressions;

namespace CompanyName.Ddd.Domain.Specifications
{
    public class NotSpecification<TEntity> : Specification<TEntity>
    {
        private readonly ISpecification<TEntity> _specification;

        public NotSpecification(ISpecification<TEntity> specification)
        {
            _specification = specification ?? throw new ArgumentNullException(nameof(specification));

            Criteria = ExpressionCombiner.Not(_specification.Criteria);

            @base.IncludeExpressions.AddRange(_specification.IncludeExpressions);
        }
    }
}
