using System.Reflection;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using CompanyName.AutoMapper;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCompanyNameAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
        {
            var callingAssembly = Assembly.GetCallingAssembly();

            services
                .AddAutoMapper(options =>
                {
                    options.AddCollectionMappers();

                }, assemblies.DefaultIfEmpty(callingAssembly))
                .AddCompanyNameAutoMapperCommon();

            return services;
        }

        public static IServiceCollection AddCompanyNameAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> configAction)
        {
            var callingAssembly = Assembly.GetCallingAssembly();

            services
                .AddAutoMapper(options =>
                {

                    configAction(options);

                    options.AddCollectionMappers();
                }, callingAssembly)
                .AddCompanyNameAutoMapperCommon();

            return services;
        }

        public static IServiceCollection AddCompanyNameAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> configAction, params Assembly[] assemblies)
        {
            var callingAssembly = Assembly.GetCallingAssembly();

            services
                .AddAutoMapper(options =>
                {

                    configAction(options);

                    options.AddCollectionMappers();


                }, assemblies.DefaultIfEmpty(callingAssembly))
                .AddCompanyNameAutoMapperCommon();

            return services;
        }

        public static IServiceCollection AddCompanyNameAutoMapper(this IServiceCollection services, Action<IServiceProvider, IMapperConfigurationExpression> configAction, params Assembly[] assemblies)
        {
            var callingAssembly = Assembly.GetCallingAssembly();

            services
                .AddAutoMapper((sp, options) =>
                {

                    configAction(sp, options);

                    options.AddCollectionMappers();

                }, assemblies.DefaultIfEmpty(callingAssembly))
                .AddCompanyNameAutoMapperCommon();

            return services;
        }

        private static IServiceCollection AddCompanyNameAutoMapperCommon(this IServiceCollection services)
        {
            services
                .AddCompanyNameCore()
                .AddSingleton<MapperAccessor>()
                .AddSingleton<IMapperAccessor>(serviceProvider => serviceProvider.GetRequiredService<MapperAccessor>());

            return services;
        }
    }
}
