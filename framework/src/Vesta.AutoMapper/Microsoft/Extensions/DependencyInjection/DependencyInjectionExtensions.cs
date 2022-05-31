using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Vesta.AutoMapper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {

        public static void AddVestaAutoMapper(this IServiceCollection services)
        {

            services.AddVestaCore();

            services.AddSingleton<MapperAccessor>(serviceProvider =>
            {
                var mapper = serviceProvider.GetRequiredService<IMapper>();
                return new MapperAccessor(mapper);

            });

            services.AddSingleton<IMapperAccessor, MapperAccessor>();

        }
    }
}
