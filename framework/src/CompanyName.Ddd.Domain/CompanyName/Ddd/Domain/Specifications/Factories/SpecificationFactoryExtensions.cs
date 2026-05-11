using System.Linq.Expressions;

namespace CompanyName.Ddd.Domain.Specifications.Factories
{
    public static class SpecificationFactoryExtensions
    {
        // Métodos de conveniencia para crear especificaciones comunes
        public static ISpecification<TEntity> Equal<TEntity, TProperty>(
            this ISpecificationFactory<TEntity> factory,
            Expression<Func<TEntity, TProperty>> property,
            TProperty value)
        {
            var parameter = property.Parameters[0];
            var body = Expression.Equal(property.Body, Expression.Constant(value));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);

            return factory.Create(lambda);
        }

        public static ISpecification<TEntity> Contains<TEntity>(
            this ISpecificationFactory<TEntity> factory,
            Expression<Func<TEntity, string>> property,
            string value)
        {
            var parameter = property.Parameters[0];
            var method = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
            var body = Expression.Call(property.Body, method, Expression.Constant(value));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);

            return factory.Create(lambda);
        }

        public static ISpecification<TEntity> GreaterThan<TEntity, TProperty>(
            this ISpecificationFactory<TEntity> factory,
            Expression<Func<TEntity, TProperty>> property,
            TProperty value) where TProperty : IComparable<TProperty>
        {
            var parameter = property.Parameters[0];
            var body = Expression.GreaterThan(property.Body, Expression.Constant(value));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);

            return factory.Create(lambda);
        }

        public static ISpecification<TEntity> LessThan<TEntity, TProperty>(
            this ISpecificationFactory<TEntity> factory,
            Expression<Func<TEntity, TProperty>> property,
            TProperty value) where TProperty : IComparable<TProperty>
        {
            var parameter = property.Parameters[0];
            var body = Expression.LessThan(property.Body, Expression.Constant(value));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);

            return factory.Create(lambda);
        }

        public static ISpecification<TEntity> In<TEntity, TProperty>(
            this ISpecificationFactory<TEntity> factory,
            Expression<Func<TEntity, TProperty>> property,
            IEnumerable<TProperty> values)
        {
            var valuesList = values.ToList();
            if (!valuesList.Any())
            {
                return factory.False();
            }

            var parameter = property.Parameters[0];
            var method = typeof(Enumerable).GetMethods()
                .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TProperty));

            var body = Expression.Call(method, Expression.Constant(valuesList), property.Body);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);

            return factory.Create(lambda);
        }
    }
}


