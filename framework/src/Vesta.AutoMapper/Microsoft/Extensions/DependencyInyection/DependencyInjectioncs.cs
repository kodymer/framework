using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Vesta.AutoMapper;

namespace Microsoft.Extensions.DependencyInyection
{
    public static class DependencyInjectioncs
    {

        public static ServiceCollection AddVestaAutoMapper(this ServiceCollection services)
        {

            services.AddSingleton<MapperAccessor>(serviceProvider =>
            {
                var mapper = serviceProvider.GetRequiredService<IMapper>();
                return new MapperAccessor(mapper);

            });

            services.AddSingleton<IMapperAccessor, MapperAccessor>();

            return services;
        }
    }
}
