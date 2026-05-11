using CompanyName.Ddd.Domain.Entities;
using System.Linq.Expressions;

namespace CompanyName.Ddd.Domain.Specifications
{
    public interface ISpecification<TEntity>
    {

        internal List<Expression<Func<TEntity, object>>> IncludeExpressions { get; }
        internal Expression<Func<TEntity, object>> OrderExpression { get; set;  }
        internal Expression<Func<TEntity, object>> OrderByDecendingExpression { get; set; }

        Expression<Func<TEntity, bool>> Criteria { get; }

        int? Take { get; set; }
        int? Skip { get; set; }
        bool IsPagingEnabled { get; }

        //ISpecification<TEntity> And(ISpecification<TEntity> specification);
        //ISpecification<TEntity> Or(ISpecification<TEntity> specification);
        //ISpecification<TEntity> Not();
    }
}
