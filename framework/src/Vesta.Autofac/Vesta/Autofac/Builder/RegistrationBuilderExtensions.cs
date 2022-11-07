using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.Autofac.Builder
{
    public static class RegistrationBuilderExtensions
    {

        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> ConfigureConventions<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrationBuilder)
            where TActivatorData : ReflectionActivatorData
        {
            var serviceType = registrationBuilder.RegistrationData.Services.OfType<IServiceWithType>().FirstOrDefault()?.ServiceType;
            if (serviceType == null)
            {
                return registrationBuilder;
            }

            var implementationType = registrationBuilder.ActivatorData.ImplementationType;
            if (implementationType == null)
            {
                return registrationBuilder;
            }

            registrationBuilder = registrationBuilder.EnablePropertyInjection(implementationType);
            registrationBuilder = registrationBuilder.AddInterceptors(serviceType, implementationType);

            return registrationBuilder;
        }


        private static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> EnablePropertyInjection<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrationBuilder, Type implementationType)
            where TActivatorData : ReflectionActivatorData
        {

            registrationBuilder = registrationBuilder.PropertiesAutowired();
            return registrationBuilder;
        }

        private static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> AddInterceptors<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrationBuilder, Type serviceType, Type implementationType)
            where TActivatorData : ReflectionActivatorData
        {

            if (serviceType.GetCustomAttributes(typeof(InterceptAttribute), true).Any() ||
                implementationType.GetCustomAttributes(typeof(InterceptAttribute), true).Any())
            {

                if (serviceType.IsInterface)
                {
                    registrationBuilder = registrationBuilder.EnableInterfaceInterceptors();
                }
                else
                {
                    (registrationBuilder as IRegistrationBuilder<TLimit, ConcreteReflectionActivatorData, TRegistrationStyle>)?.EnableClassInterceptors();
                }

            }

            return registrationBuilder;
        }
    }
}
