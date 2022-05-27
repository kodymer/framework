using Ardalis.GuardClauses;

namespace Vesta.Core.Reflection
{
    public static class TypeExtensíons
    {
        public static bool IsAssignableTo<TTarget>(this Type type)
        {
            Guard.Against.Null(type, nameof(type));

            return type.IsAssignableTo(typeof(TTarget));
        }

        public static bool IsAssignableTo<TTarget>(this Type type, Type targetType)
        {
            Guard.Against.Null(type, nameof(type));
            Guard.Against.Null(targetType, nameof(targetType));

            return targetType.IsAssignableFrom(type);
        }
    }
}