using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.Ddd.Domain.Specifications.Factories
{
    public static class SpecificationExtensions
    {
        public static ISpecification<TEntity> And<TEntity>(
            this ISpecification<TEntity> left,
            ISpecification<TEntity> right)
        {
            return new AndSpecification<TEntity>(left, right);
        }

        public static ISpecification<TEntity> Or<TEntity>(
            this ISpecification<TEntity> left,
            ISpecification<TEntity> right)
        {
            return new OrSpecification<TEntity>(left, right);
        }

        public static ISpecification<TEntity> Not<TEntity>(
            this ISpecification<TEntity> specification)
        {
            return new NotSpecification<TEntity>(specification);
        }
    }
}


