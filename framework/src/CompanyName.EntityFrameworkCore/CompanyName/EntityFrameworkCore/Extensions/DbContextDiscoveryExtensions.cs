using CompanyName.EntityFrameworkCore;
using System.Reflection;

namespace CompanyName.EntityFrameworkCore.Extensions
{
    public static class DbContextDiscoveryExtensions
    {
        public static IEnumerable<Type> GetDbContexts(this Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(type =>
                    type.IsClass &&
                    !type.IsAbstract &&
                    InheritsFromGenericBase(type, typeof(CompanyNameDbContextBase<>)));
        }

        private static bool InheritsFromGenericBase(Type type, Type genericBase)
        {
            while (type != null && type != typeof(object))
            {
                var current = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (current == genericBase)
                    return true;
                type = type.BaseType;
            }
            return false;
        }
    }
}
