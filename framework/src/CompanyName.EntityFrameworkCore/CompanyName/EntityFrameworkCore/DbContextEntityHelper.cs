using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CompanyName.EntityFrameworkCore
{

    public static class DbContextEntityHelper
    {
        public static bool IsEntityConfigured<TContext>(Type entityClrType)
            where TContext : DbContext
        {
            return IsEntityConfigured(typeof(TContext), entityClrType);
        }

        public static bool IsEntityConfigured(Type dbContextType, Type entityClrType)
        {
            if (!typeof(DbContext).IsAssignableFrom(dbContextType))
                throw new ArgumentException("The type must inherit from DbContext.", nameof(dbContextType));

            var options = CreateOptions(dbContextType);

            // Create an instance of the DbContext
            var context = (DbContext)Activator.CreateInstance(dbContextType, options);
            if (context == null)
                throw new InvalidOperationException("Could not create an instance of the DbContext.");

            // Check if the entity is configured by comparing ClrType
            var isConfigured = context.Model.GetEntityTypes()
                .Any(e => e.ClrType == entityClrType);

            return isConfigured;
        }

        public static List<Type> GetConfiguredEntities(Type dbContextType)
        {
            if (!typeof(DbContext).IsAssignableFrom(dbContextType))
                throw new ArgumentException("The type must inherit from DbContext.", nameof(dbContextType));

            var options = CreateOptions(dbContextType);

            // Create an instance of the DbContext
            var context = (DbContext)Activator.CreateInstance(dbContextType, options);
            if (context == null)
                throw new InvalidOperationException("Could not create an instance of the DbContext.");

            // Check if the entity is configured by comparing ClrType
            var entities = context.Model.GetEntityTypes().Select(e => e.ClrType).ToList();
            return entities;
        }

        private static object CreateOptions(Type dbContextType)
        {

            var optionsBuilderType = typeof(DbContextOptionsBuilder<>).MakeGenericType(dbContextType);
            var optionsBuilder = Activator.CreateInstance(optionsBuilderType);

            var useInMemoryMethod = typeof(InMemoryDbContextOptionsExtensions).GetMethods()
                .FirstOrDefault(m =>
                    m.Name == "UseInMemoryDatabase" &&
                    m.IsGenericMethod &&
                    m.GetParameters().Length == 3);

            useInMemoryMethod?
                .MakeGenericMethod(dbContextType)
                .Invoke(optionsBuilder, new object[] { optionsBuilder, "TestDb", null });

            if (useInMemoryMethod == null)
                throw new InvalidOperationException("The method 'UseInMemoryDatabase' could not be found.");


            var optionsProperty = optionsBuilderType.GetProperty("Options", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            if (optionsProperty == null)
                throw new InvalidOperationException("The property 'Options' could not be found.");

            var options = optionsProperty?.GetValue(optionsBuilder);
            return options;
        }
    }
}


