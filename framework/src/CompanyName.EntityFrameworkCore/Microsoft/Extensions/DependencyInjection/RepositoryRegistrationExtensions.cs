using CommunityToolkit.Diagnostics;
using CompanyName.AutoMapper;
using CompanyName.Core.DependencyInjection.Extensions;
using CompanyName.Ddd.Domain.Repositories;
using CompanyName.EntityFrameworkCore;
using CompanyName.EntityFrameworkCore.Abstractions;
using CompanyName.EntityFrameworkCore.Repositories;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RepositoryRegistrationExtensions
    {

        public static IServiceCollection AddRepository<TRepository, TConcreteRepository, TContext>(this IServiceCollection services)
           where TConcreteRepository : TRepository
           where TContext : CompanyNameDbContextBase<TContext>
        {
            AddRepository<TContext>(services, typeof(TRepository), typeof(TConcreteRepository));

            return services;
        }

        public static IServiceCollection AddRepository<TContext>(this IServiceCollection services, Type repositoryType, Type concreteRepositoryType)
            where TContext : CompanyNameDbContextBase<TContext>
        {
            Guard.IsTrue(
                typeof(IRepository).IsAssignableFrom(repositoryType),
                nameof(repositoryType),
                $"The type '{repositoryType.FullName}' must inherit from IRepository or IReadRepository<T>."
            );

            Guard.IsTrue(
                repositoryType.IsAssignableFrom(concreteRepositoryType),
                nameof(concreteRepositoryType),
                $"The type '{concreteRepositoryType.FullName}' must implement the interface '{repositoryType.FullName}'."
            );

            Guard.IsTrue(
                concreteRepositoryType.IsClass && !concreteRepositoryType.IsAbstract,
                nameof(concreteRepositoryType),
                $"The type '{concreteRepositoryType.FullName}' must be a non-abstract class."
            );

            Guard.IsTrue(
                typeof(IRepository).IsAssignableFrom(concreteRepositoryType),
                nameof(concreteRepositoryType),
                $"The type '{concreteRepositoryType.FullName}' must implement IRepository directly or indirectly."
            );

            if (!services.Any(repositoryType))
            {
                services
                    .AddTransient(repositoryType, serviceProvider =>
                    {
                        return CreateRepository<TContext>(serviceProvider, repositoryType, concreteRepositoryType);
                    });
            }

            //services
            //    .AddKeyedTransient(repositoryType, typeof(TContext).Name, (serviceProvider, key) =>
            //    {
            //        return CreateRepository<TContext>(serviceProvider, repositoryType, concreteRepositoryType);
            //    });

            return services;
        }

        //public static IServiceCollection AddRepositories(this IServiceCollection services, Assembly assembly = null)
        //{
        //    assembly ??= Assembly.GetCallingAssembly();

        //    var contextTypes = assembly.GetDbContexts();
        //    foreach (var contextType in contextTypes)
        //    {
        //        var entities = DbContextEntityHelper.GetConfiguredEntities(contextType);
        //        foreach (var entity in entities)
        //        {
        //            if (!entity.IsAbstract && !entity.IsInterface)
        //            {
        //                var repositoryType = typeof(IRepository<>).MakeGenericType(entity);

        //                var readRepositoryType = typeof(IReadRepository<>).MakeGenericType(entity);

        //                Type concreteRepositoryType = null;

        //                MethodInfo addRepository = null;

        //                switch (entity)
        //                {
        //                    case Type aggregateRootType when aggregateRootType.IsAssignableTo(typeof(IAggregateRoot)):

        //                        concreteRepositoryType = typeof(Repository<,>).MakeGenericType(contextType, entity);

        //                        addRepository = typeof(RepositoryRegistrationExtensions)
        //                            .GetMethod(nameof(AddRepository), new[] { typeof(IServiceCollection), typeof(Type), typeof(Type) })?
        //                            .MakeGenericMethod(contextType);

        //                        addRepository.Invoke(null, new object[] { services, repositoryType, concreteRepositoryType });
        //                        addRepository.Invoke(null, new object[] { services, readRepositoryType, concreteRepositoryType });

        //                        break;
        //                    case Type entityType when entityType.IsAssignableTo(typeof(Entity)):

        //                        concreteRepositoryType = typeof(ReadRepository<,>).MakeGenericType(contextType, entity);

        //                        addRepository = typeof(RepositoryRegistrationExtensions)
        //                            .GetMethod(nameof(AddRepository), new[] { typeof(IServiceCollection) })?
        //                            .MakeGenericMethod(readRepositoryType, concreteRepositoryType, contextType);

        //                        addRepository.Invoke(null, new object[] { services });
        //                        break;
        //                }
        //            }
        //        }
        //    }

        //    return services;
        //}

        public static IServiceCollection AddRepositories<TContext>(this IServiceCollection services, Assembly assembly = null)
             where TContext : CompanyNameDbContextBase<TContext>
        {
            assembly ??= Assembly.GetCallingAssembly();

            var concreteRepositoryTypes = assembly
                .DefinedTypes
                .Where(type => type.IsAssignableTo(typeof(IRepository)) && !type.IsAbstract && !type.IsInterface)
                .ToArray();

            foreach (var concreteRepositoryType in concreteRepositoryTypes)
            {
                var implementedInterfaces = concreteRepositoryType
                    .GetInterfaces()
                    .Where(IsValidRepositoryContract)
                    .ToArray();

                if (TryGetEntityType(implementedInterfaces, out var entityType) &&
                    !DbContextEntityHelper.IsEntityConfigured<TContext>(entityType))
                {
                    continue;
                }

                foreach (var @interface in implementedInterfaces)
                {

                    services
                        .AddRepository<TContext>(@interface, concreteRepositoryType);
                }
            }

            return services;

            bool IsValidRepositoryContract(Type @interface)
            {
                if (IsGenericRepositoryInterface(@interface))
                {
                    return true;
                }


                if (IsNotGenericRepositoryInterface(@interface))
                {
                    return true;
                }

                return false;
            }

            bool IsGenericRepositoryInterface(Type @interface)
            {
                var baseType = typeof(IReadOnlyRepository<>);

                return
                    (@interface.IsGenericType &&
                     @interface.GetGenericTypeDefinition().IsAssignableTo(baseType)) ||
                     @interface.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == baseType);
            }

            bool IsNotGenericRepositoryInterface(Type @interface)
            {
                return
                    !@interface.IsGenericType &&
                    !@interface.IsAssignableTo(typeof(IRepository));
            }


            bool TryGetEntityType(Type[] @interfaces, out Type entityType)
            {
                entityType = null;
                foreach (var @interface in @interfaces)
                {
                    if (IsGenericRepositoryInterface(@interface))
                    {
                        entityType = @interface.GetGenericArguments()[0];
                        return true;
                    }
                    else
                    {
                        return TryGetEntityType(@interface.GetInterfaces(), out entityType);
                    }
                }

                return false;
            }
        }

        private static object CreateRepository<TContext>(IServiceProvider serviceProvider, Type repositoryType, Type concreteRepositoryType)
               where TContext : CompanyNameDbContextBase<TContext>
        {
            var contextProvider = serviceProvider.GetRequiredService<IDbContextProvider<TContext>>();
            var mapperAccessor = serviceProvider.GetRequiredService<IMapperAccessor>();

            var repositoryFactoryType = typeof(EfCoreRepositoryFactory<,,>).MakeGenericType(repositoryType, concreteRepositoryType, typeof(TContext));
            var createRepository = repositoryFactoryType.GetMethod("CreateRepository");

            var args = new object[] { contextProvider };
            var repositoryFactory = Activator.CreateInstance(repositoryFactoryType, args)!;
            var repositoryInstance = createRepository.Invoke(repositoryFactory, new object[] { });

            BindingFlags bindingAttr = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty;
            var configurationProvider = concreteRepositoryType.GetProperty("ConfigurationProvider", bindingAttr);
            configurationProvider.SetValue(repositoryInstance, mapperAccessor.Mapper.ConfigurationProvider);

            return repositoryInstance;
        }
    }
}
