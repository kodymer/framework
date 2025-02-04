using AutoMapper;
using AutoMapper.Collection;
using AutoMapper.EquivalencyExpression;
using System.Reflection;
using CompanyName.AutoMapper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCompanyNameAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddAutoMapper(options =>
            {
                options.AddCollectionMappers();

            }, assemblies);

            services.AddCompanyNameAutoMapperCommon();
        }

        public static void AddCompanyNameAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> configAction)
        {
            services.AddAutoMapper(configAction);

            services.AddCompanyNameAutoMapperCommon();
        }

        public static void AddCompanyNameAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> configAction, params Assembly[] assemblies)
        {
            services.AddAutoMapper(configAction, assemblies);

            services.AddCompanyNameAutoMapperCommon();
        }

        public static void AddCompanyNameAutoMapper(this IServiceCollection services, Action<IServiceProvider, IMapperConfigurationExpression> configAction, params Assembly[] assemblies)
        {
            services.AddAutoMapper(configAction, assemblies);

            services.AddCompanyNameAutoMapperCommon();
        }

        private static void AddCompanyNameAutoMapperCommon(this IServiceCollection services)
        {
            services.AddCompanyNameCore();

            services.AddSingleton<MapperAccessor>();
            services.AddSingleton<IMapperAccessor>(serviceProvider => serviceProvider.GetRequiredService<MapperAccessor>());
        }
    }
}
