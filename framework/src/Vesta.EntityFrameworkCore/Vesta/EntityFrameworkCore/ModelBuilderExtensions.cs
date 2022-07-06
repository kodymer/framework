using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vesta.Core;

namespace Vesta.EntityFrameworkCore
{
    public static class ModelBuilderExtensions
    {
        public static void ApplySoftDeleteQueryFilterConcept(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var body = Expression.NotEqual(Expression.Property(parameter, nameof(ISoftDelete.IsDeleted)), Expression.Constant(true));
                    var softDeletePredicate = Expression.Lambda(body, parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(softDeletePredicate);
                }
            }
        }
    }
}
