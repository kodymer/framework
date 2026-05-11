using CompanyName.Core.Utilities.Expressions;
using System.Linq.Expressions;

namespace CompanyName.Ddd.Domain.Specifications.Factories
{
    public class SpecificationBuilder<TEntity> : ISpecificationBuilder<TEntity>
    {
        private Expression<Func<TEntity, bool>> _criteria;
        private readonly List<Expression<Func<TEntity, object>>> _includes = new();
        private Expression<Func<TEntity, object>> _orderBy;
        private Expression<Func<TEntity, object>> _orderByDescending;
        private int? _skip;
        private int? _take;

        public ISpecificationBuilder<TEntity> Where(Expression<Func<TEntity, bool>> criteria)
        {
            _criteria = criteria;
            return this;
        }

        public ISpecificationBuilder<TEntity> And(Expression<Func<TEntity, bool>> criteria)
        {
            if (_criteria == null)
            {
                _criteria = criteria;
            }
            else
            {
                _criteria = ExpressionCombiner.And(_criteria, criteria);
            }
            return this;
        }

        public ISpecificationBuilder<TEntity> Or(Expression<Func<TEntity, bool>> criteria)
        {
            if (_criteria == null)
            {
                _criteria = criteria;
            }
            else
            {
                _criteria = ExpressionCombiner.Or(_criteria, criteria);
            }
            return this;
        }

        public ISpecificationBuilder<TEntity> Not(Expression<Func<TEntity, bool>> criteria)
        {
            var notCriteria = ExpressionCombiner.Not(criteria);

            if (_criteria == null)
            {
                _criteria = notCriteria;
            }
            else
            {
                _criteria = ExpressionCombiner.And(_criteria, notCriteria);
            }
            return this;
        }

        public ISpecificationBuilder<TEntity> Include(Expression<Func<TEntity, object>> includeExpression)
        {
            _includes.Add(includeExpression);
            return this;
        }

        public ISpecificationBuilder<TEntity> OrderBy(Expression<Func<TEntity, object>> orderExpression)
        {
            _orderBy = orderExpression;
            _orderByDescending = null; // Reset descending order
            return this;
        }

        public ISpecificationBuilder<TEntity> OrderByDescending(Expression<Func<TEntity, object>> orderExpression)
        {
            _orderByDescending = orderExpression;
            _orderBy = null; // Reset ascending order
            return this;
        }

        public ISpecificationBuilder<TEntity> Skip(int skip)
        {
            _skip = skip;
            return this;
        }

        public ISpecificationBuilder<TEntity> Take(int take)
        {
            _take = take;
            return this;
        }

        public ISpecificationBuilder<TEntity> Paginate(int page, int pageSize)
        {
            _skip = (page - 1) * pageSize;
            _take = pageSize;
            return this;
        }

        public ISpecification<TEntity> Build()
        {
            var criteria = _criteria ?? (x => true);
            var specification = new FluentSpecification<TEntity>(criteria);

            // Aplicar includes
            foreach (var include in _includes)
            {
                specification.AddInclude(include);
            }

            // Aplicar ordenamiento
            if (_orderBy != null)
            {
                specification.ApplyOrderBy(_orderBy);
            }
            else if (_orderByDescending != null)
            {
                specification.ApplyOrderByDescending(_orderByDescending);
            }

            // Aplicar paginación
            if (_skip.HasValue || _take.HasValue)
            {
                specification.ApplyPaging(_skip ?? 0, _take ?? int.MaxValue);
            }

            return specification;
        }
    }
}


