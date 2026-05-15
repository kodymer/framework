
using AutoMapper;
using System.Collections;
using System.Reflection;

namespace CompanyName.AutoMapper
{
    /// <summary>
    /// AutoMapper extension that ignores destination members which either:
    ///  - do not exist in the source type,
    ///  - are not writable (no public setter),
    ///  - or are deemed non-mappable by simple heuristics (e.g., incompatible complex/simple types, unmatched collections).
    /// It also allows explicit exceptions by name and an optional predicate for custom ignore rules.
    /// </summary>
    public static class MapperExtensions
    {
        private static readonly BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance;

        /// <summary>
        /// Determines whether a type is considered "simple" for mapping purposes.
        /// Simple types are mapped as values; complex types usually require their own maps.
        /// </summary>
        private static bool IsSimpleType(Type t) =>
            t.IsPrimitive
            || t.IsEnum
            || t == typeof(string)
            || t == typeof(decimal)
            || t == typeof(DateTime)
            || t == typeof(DateTimeOffset)
            || t == typeof(TimeSpan)
            || t == typeof(Guid);

        /// <summary>
        /// Returns true if the type implements IEnumerable and is not a string.
        /// </summary>
        private static bool IsEnumerableButNotString(Type t) =>
            t != typeof(string) && typeof(IEnumerable).IsAssignableFrom(t);

        // Cache of source property names by source type to reduce reflection cost.
        private static readonly Dictionary<Type, HashSet<string>> SourcePropNameCache = new();

        private static HashSet<string> GetSourcePropNames(Type sourceType)
        {
            if (SourcePropNameCache.TryGetValue(sourceType, out var names))
                return names;

            names = sourceType
                .GetProperties(Flags)
                .Select(p => p.Name)
                .ToHashSet(StringComparer.Ordinal);

            SourcePropNameCache[sourceType] = names;
            return names;
        }

        /// <summary>
        /// Ignores destination members that don't exist in the source or are not mappable
        /// according to heuristics (complex/simple mismatch, unmatched collections, non-writable members).
        /// Allows explicit exceptions by name and a custom predicate.
        /// </summary>
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> expression,
            IEnumerable<string> alwaysIgnoreNames = null,
            Func<PropertyInfo, bool> extraIgnorePredicate = null)
        {
            var sourceType = typeof(TSource);
            var destType = typeof(TDestination);

            var sourcePropNames = GetSourcePropNames(sourceType);
            var destProps = destType.GetProperties(Flags);

            var explicitIgnore = new HashSet<string>(alwaysIgnoreNames ?? Enumerable.Empty<string>(), StringComparer.Ordinal);

            foreach (var destProp in destProps)
            {
                // 1) Special member by name or matched by custom predicate
                if (explicitIgnore.Contains(destProp.Name) || (extraIgnorePredicate?.Invoke(destProp) ?? false))
                {
                    expression.ForMember(destProp.Name, opt => opt.Ignore());
                    continue;
                }

                // 2) If the member does not exist on the source, ignore
                if (!sourcePropNames.Contains(destProp.Name))
                {
                    expression.ForMember(destProp.Name, opt => opt.Ignore());
                    continue;
                }

                // 3) If it exists, inspect type compatibility
                var srcProp = sourceType.GetProperty(destProp.Name, Flags);
                if (srcProp is null)
                {
                    expression.ForMember(destProp.Name, opt => opt.Ignore());
                    continue;
                }

                var srcType = srcProp.PropertyType;
                var dstType = destProp.PropertyType;

                // 4) If destination is not writable (no public setter), ignore
                if (!destProp.CanWrite)
                {
                    expression.ForMember(destProp.Name, opt => opt.Ignore());
                    continue;
                }

                // 5) Collections: if both sides are IEnumerable (not string), keep; otherwise ignore to avoid odd mappings
                var srcEnumerable = IsEnumerableButNotString(srcType);
                var dstEnumerable = IsEnumerableButNotString(dstType);
                if (srcEnumerable || dstEnumerable)
                {
                    var bothEnumerable = srcEnumerable && dstEnumerable;
                    if (!bothEnumerable)
                    {
                        expression.ForMember(destProp.Name, opt => opt.Ignore());
                    }
                    continue;
                }

                // 6) Complex/simple mismatch: if one is complex and the other is simple, ignore
                var srcComplex = !IsSimpleType(srcType);
                var dstComplex = !IsSimpleType(dstType);
                if (srcComplex ^ dstComplex) // XOR: only one is complex
                {
                    expression.ForMember(destProp.Name, opt => opt.Ignore());
                    continue;
                }

                // 7) Clear incompatibilities (e.g., string -> int) — ignore unless you have a specific type converter registered
                if (!TypesCompatible(srcType, dstType))
                {
                    expression.ForMember(destProp.Name, opt => opt.Ignore());
                    continue;
                }
            }

            return expression;
        }

        /// <summary>
        /// Very conservative compatibility check.
        /// Returns true if destination is assignable from source or types are equal.
        /// Extend this if you allow implicit conversions or have custom TypeConverters.
        /// </summary>
        private static bool TypesCompatible(Type src, Type dst)
        {
            if (dst.IsAssignableFrom(src)) return true;
            if (src == dst) return true;
            return false;
        }
    }
}
