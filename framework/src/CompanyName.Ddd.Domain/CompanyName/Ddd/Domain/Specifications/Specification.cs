using System.Linq.Expressions;
using System.Runtime.CompilerServices;

[assembly:
    InternalsVisibleTo("CompanyName.EntityFrameworkCore")
]

namespace CompanyName.Ddd.Domain.Specifications
{
    public abstract class Specification<TEntity> : ISpecification<TEntity>
    {
        protected ISpecification<TEntity> @base => this.As<ISpecification<TEntity>>();

        protected Specification()
        {
        }

        protected Specification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
        }

        protected ISpecification<TEntity> AddInclude(Expression<Func<TEntity, object>> expression)
        {
            @base.IncludeExpressions.Add(expression);

            return this;
        }

        protected ISpecification<TEntity> ApplyOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            @base.OrderExpression = orderByExpression;

            return this;
        }

        protected ISpecification<TEntity> ApplyOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        {
            @base.OrderByDecendingExpression = orderByDescendingExpression;

            return this;
        }

        protected virtual void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

        List<Expression<Func<TEntity, object>>> ISpecification<TEntity>.IncludeExpressions => new();

        Expression<Func<TEntity, object>> ISpecification<TEntity>.OrderExpression { get; set; }

        Expression<Func<TEntity, object>> ISpecification<TEntity>.OrderByDecendingExpression { get; set; }


        public virtual Expression<Func<TEntity, bool>> Criteria { get; protected set; } = null!;

        public int? Take { get; set; }

        public int? Skip { get; set; }

        public bool IsPagingEnabled => Skip.HasValue;

        public static implicit operator Expression<Func<TEntity, bool>>(Specification<TEntity> specification)
        {
            return specification.Criteria;
        }

        public static Specification<TEntity> operator &(Specification<TEntity> left, Specification<TEntity> right)
        {
            return new AndSpecification<TEntity>(left, right);
        }

        public static Specification<TEntity> operator |(Specification<TEntity> left, Specification<TEntity> right)
        {
            return new OrSpecification<TEntity>(left, right);
        }

        public static Specification<TEntity> operator !(Specification<TEntity> specification)
        {
            return new NotSpecification<TEntity>(specification);
        }
    }
}
