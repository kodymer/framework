using System.Reflection;
using CommunityToolkit.Diagnostics;

namespace CompanyName.Core.Reflection
{
    public static class TypeExtensions
    {
        public static bool IsAssignableTo<TTarget>(this Type type)
        {
            Guard.IsNotNull(type);

            return type.IsAssignableTo(typeof(TTarget));
        }

        public static bool IsAssignableTo(this Type type, Type targetType)
        {
            Guard.IsNotNull(type);
            Guard.IsNotNull(targetType);

            return targetType.IsAssignableFrom(type);
        }

        public static bool TryGetAttribute<TAttr>(this Type type, out object attr)
        {
            attr = type.GetTypeInfo().GetCustomAttributes(typeof(TAttr), false).FirstOrDefault();
            if (!(attr is null))
            {
                return true;
            }

            return false;
        }
        public static bool TryGetPropertyValue<TPropertyValue>(this Type type, object instance, string propertyName, out TPropertyValue propertyValue)
        {
            var propertyInfo = type.GetTypeInfo().GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            propertyValue = default(TPropertyValue);
            if (!(propertyInfo is null))
            {
                propertyValue = (TPropertyValue)propertyInfo.GetValue(instance);
                return true;
            }

            return false;
        }
    }
}