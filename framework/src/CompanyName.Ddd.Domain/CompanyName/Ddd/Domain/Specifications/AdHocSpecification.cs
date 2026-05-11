using System.Linq.Expressions;

namespace CompanyName.Ddd.Domain.Specifications
{
    public class AdHocSpecification<TEntity> : Specification<TEntity>
    {
        public AdHocSpecification(Expression<Func<TEntity, bool>> criteria)
            : base(criteria)
        {
        }
    }
}
