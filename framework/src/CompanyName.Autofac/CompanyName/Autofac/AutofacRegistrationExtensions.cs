//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using CompanyName.Autofac.Builder;
using CompanyName.Autofac.Extensions.DependencyInjection;

namespace CompanyName.Autofac
{
    /// <summary>
    /// Extension methods for registering ASP.NET Core dependencies with Autofac.
    /// </summary>
    public static class AutofacRegistrationExtensions
    {
        /// <summary>
        /// Populates the Autofac container builder with the set of registered service descriptors
        /// and makes <see cref="IServiceProvider"/> and <see cref="IServiceScopeFactory"/>
        /// available in the container.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="ContainerBuilder"/> into which the registrations should be made.
        /// </param>
        /// <param name="descriptors">
        /// The set of service descriptors to register in the container.
        /// </param>
        public static void Populate(
            this ContainerBuilder builder,
            IEnumerable<ServiceDescriptor> descriptors)
        {
            builder.Populate(descriptors, null);
        }

        /// <summary>
        /// Populates the Autofac container builder with the set of registered service descriptors
        /// and makes <see cref="IServiceProvider"/> and <see cref="IServiceScopeFactory"/>
        /// available in the container. Using this overload is incompatible with the ASP.NET Core
        /// support for <see cref="IServiceProviderFactory{TContainerBuilder}"/>.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="ContainerBuilder"/> into which the registrations should be made.
        /// </param>
        /// <param name="descriptors">
        /// The set of service descriptors to register in the container.
        /// </param>
        /// <param name="lifetimeScopeTagForSingletons">
        /// If provided and not <see langword="null"/> then all registrations with lifetime <see cref="ServiceLifetime.Singleton" /> are registered
        /// using <see cref="IRegistrationBuilder{TLimit,TActivatorData,TRegistrationStyle}.InstancePerMatchingLifetimeScope" />
        /// with provided <paramref name="lifetimeScopeTagForSingletons"/>
        /// instead of using <see cref="IRegistrationBuilder{TLimit,TActivatorData,TRegistrationStyle}.SingleInstance"/>.
        /// </param>
        /// <remarks>
        /// <para>
        /// Specifying a <paramref name="lifetimeScopeTagForSingletons"/> addresses a specific case where you have
        /// an application that uses Autofac but where you need to isolate a set of services in a child scope. For example,
        /// if you have a large application that self-hosts ASP.NET Core items, you may want to isolate the ASP.NET
        /// Core registrations in a child lifetime scope so they don't show up for the rest of the application.
        /// This overload allows that. Note it is the developer's responsibility to execute this and create an
        /// <see cref="AutofacServiceProvider"/> using the child lifetime scope.
        /// </para>
        /// </remarks>
        public static void Populate(
            this ContainerBuilder builder,
            IEnumerable<ServiceDescriptor> descriptors,
            object lifetimeScopeTagForSingletons)
        {
            if (descriptors == null)
            {
                throw new ArgumentNullException(nameof(descriptors));
            }

            builder
                .RegisterType<CompanyNameAutofacServiceProvider>()
                .As<IServiceProvider>()
                .ExternallyOwned();

            var autofacServiceScopeFactory = typeof(AutofacServiceProvider).Assembly.GetType("Autofac.Extensions.DependencyInjection.AutofacServiceScopeFactory");
            if (autofacServiceScopeFactory == null)
            {
                throw new InvalidOperationException("Unable to get type of Autofac.Extensions.DependencyInjection.AutofacServiceScopeFactory!");
            }

            builder.RegisterType(autofacServiceScopeFactory).As<IServiceScopeFactory>();

            Register(builder, descriptors, lifetimeScopeTagForSingletons);
        }


        /// <summary>
        /// Populates the Autofac container builder with the set of registered service descriptors.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="ContainerBuilder"/> into which the registrations should be made.
        /// </param>
        /// <param name="descriptors">
        /// The set of service descriptors to register in the container.
        /// </param>
        /// <param name="lifetimeScopeTagForSingletons">
        /// If not <see langword="null"/> then all registrations with lifetime <see cref="ServiceLifetime.Singleton" /> are registered
        /// using <see cref="IRegistrationBuilder{TLimit,TActivatorData,TRegistrationStyle}.InstancePerMatchingLifetimeScope" />
        /// with provided <paramref name="lifetimeScopeTagForSingletons"/>
        /// instead of using <see cref="IRegistrationBuilder{TLimit,TActivatorData,TRegistrationStyle}.SingleInstance"/>.
        /// </param>
        [SuppressMessage("CA2000", "CA2000", Justification = "Registrations created here are disposed when the built container is disposed.")]
        private static void Register(
            ContainerBuilder builder,
            IEnumerable<ServiceDescriptor> descriptors,
            object lifetimeScopeTagForSingletons)
        {
            foreach (var descriptor in descriptors)
            {
                if (!descriptor.IsKeyedService)
                {
                    if (descriptor.ImplementationType != null)
                    {
                        // Test if the an open generic type is being registered
                        var serviceTypeInfo = descriptor.ServiceType.GetTypeInfo();
                        if (serviceTypeInfo.IsGenericTypeDefinition)
                        {
                            builder
                               .RegisterGeneric(descriptor.ImplementationType)
                               .As(descriptor.ServiceType)
                               .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons)
                               .ConfigureConventions();


                        }
                        else
                        {
                            builder
                               .RegisterType(descriptor.ImplementationType)
                               .As(descriptor.ServiceType)
                               .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons)
                               .ConfigureConventions();

                        }
                    }
                    else if (descriptor.ImplementationFactory != null)
                    {
                        var registration = RegistrationBuilder.ForDelegate(descriptor.ServiceType, (context, parameters) =>
                        {
                            var serviceProvider = context.Resolve<IServiceProvider>();
                            return descriptor.ImplementationFactory(serviceProvider);
                        })
                        .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons)
                        .CreateRegistration();

                        builder.RegisterComponent(registration);
                    }
                    else
                    {
                        builder
                            .RegisterInstance(descriptor.ImplementationInstance)
                            .As(descriptor.ServiceType)
                            .ConfigureLifecycle(descriptor.Lifetime, null);
                    }
                } 
                else
                {
                    if (descriptor.KeyedImplementationType != null)
                    {
                        // Test if the an open generic type is being registered
                        var serviceTypeInfo = descriptor.ServiceType.GetTypeInfo();
                        if (serviceTypeInfo.IsGenericTypeDefinition)
                        {
                            builder
                               .RegisterGeneric(descriptor.KeyedImplementationType)
                               .Keyed(descriptor.ServiceKey, descriptor.ServiceType)
                               .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons)
                               .ConfigureConventions();
                        }
                        else
                        {
                            builder
                               .RegisterType(descriptor.KeyedImplementationType)
                               .Keyed(descriptor.ServiceKey, descriptor.ServiceType)
                               .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons)
                               .ConfigureConventions();

                        }
                    }
                    else if (descriptor.KeyedImplementationFactory != null)
                    {
                        var registration = RegistrationBuilder.ForDelegate(descriptor.ServiceType, (context, parameters) =>
                        {
                            var serviceProvider = context.Resolve<IServiceProvider>();
                            return descriptor.KeyedImplementationFactory(serviceProvider, descriptor.ServiceKey);
                        })
                        .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons)
                        .CreateRegistration();

                        builder.RegisterComponent(registration);
                    }
                    else
                    {
                        builder
                            .RegisterInstance(descriptor.KeyedImplementationInstance)
                            .Keyed(descriptor.ServiceKey, descriptor.ServiceType)
                            .ConfigureLifecycle(descriptor.Lifetime, null);
                    }

                }
            }
        }
    }
}
