using CompanyName.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CompanyName.EntityFrameworkCore
{
    public static class SoftDeleteQueryFilterApplier
    {
        public static void ApplySoftDeleteQueryFilterConcept(this ModelBuilder modelBuilder)
        {
            var softDeleteEntities = GetSoftDeleteEntities(modelBuilder);

            foreach (var entityType in softDeleteEntities)
            {
                var filter = BuildSoftDeleteFilter(entityType);
                modelBuilder.Entity(entityType).HasQueryFilter(filter);
            }
        }

        private static IEnumerable<Type> GetSoftDeleteEntities(ModelBuilder modelBuilder)
        {
            return modelBuilder.Model
                .GetEntityTypes()
                .Select(e => e.ClrType)
                .Where(t => typeof(ISoftDelete).IsAssignableFrom(t));
        }

        private static LambdaExpression BuildSoftDeleteFilter(Type entityType)
        {
            var parameter = Expression.Parameter(entityType, "e");
            var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
            var condition = Expression.NotEqual(property, Expression.Constant(true));
            return Expression.Lambda(condition, parameter);
        }
    }

}
