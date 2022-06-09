using AutoMapper;
using AutoMapper.Collection;
using AutoMapper.EquivalencyExpression;
using System.Reflection;
using Vesta.AutoMapper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddAutoMapper(options =>
            {
                options.AddCollectionMappers();

            }, assemblies);

            services.AddVestaAutoMapperCommon();
        }

        public static void AddVestaAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> configAction)
        {
            services.AddAutoMapper(configAction);

            services.AddVestaAutoMapperCommon();
        }

        public static void AddVestaAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> configAction, params Assembly[] assemblies)
        {
            services.AddAutoMapper(configAction, assemblies);

            services.AddVestaAutoMapperCommon();
        }

        public static void AddVestaAutoMapper(this IServiceCollection services, Action<IServiceProvider, IMapperConfigurationExpression> configAction, params Assembly[] assemblies)
        {
            services.AddAutoMapper(configAction, assemblies);

            services.AddVestaAutoMapperCommon();
        }

        private static void AddVestaAutoMapperCommon(this IServiceCollection services)
        {
            services.AddVestaCore();

            services.AddSingleton<MapperAccessor>();
            services.AddSingleton<IMapperAccessor, MapperAccessor>();
        }
    }
}
