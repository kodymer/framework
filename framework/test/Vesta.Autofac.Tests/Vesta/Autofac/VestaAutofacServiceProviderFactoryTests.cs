using Autofac;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using Vesta.Autofac.Extensions.DependencyInjection;
using System;
using Vesta.TestBase;

namespace Vesta.Autofac
{
    public class VestaAutofacServiceProviderFactoryTests
    {
        [Trait("Category", VestaUnitTestCategories.DependencyInjection)]
        [Trait("Class", nameof(VestaAutofacServiceProviderFactory))]
        [Trait("Method", nameof(VestaAutofacServiceProviderFactory.CreateBuilder))]
        [Fact]
        public void Given_Services_When_CreateBuilder_Then_ReturnBuilder()
        {
            var services = new ServiceCollection();
            services.AddTransient<VestaService>();
            var containerBuilder = new ContainerBuilder();

            var factory = new VestaAutofacServiceProviderFactory(containerBuilder);
            var builder = factory.CreateBuilder(services);

            builder.Should().Be(containerBuilder);
            builder.Build().ResolveOptional<VestaService>().Should().NotBeNull();
        }

        [Trait("Category", VestaUnitTestCategories.DependencyInjection)]
        [Trait("Class", nameof(VestaAutofacServiceProviderFactory))]
        [Trait("Method", nameof(VestaAutofacServiceProviderFactory.CreateBuilder))]
        [Fact]
        public void Given_Null_When_CreateBuilder_Then_ThrowArgumentError()
        {
            var containerBuilder = new ContainerBuilder();

            var factory = new VestaAutofacServiceProviderFactory(containerBuilder);
            var action = () => factory.CreateBuilder(null);

            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Trait("Category", VestaUnitTestCategories.DependencyInjection)]
        [Trait("Class", nameof(VestaAutofacServiceProviderFactory))]
        [Trait("Method", nameof(VestaAutofacServiceProviderFactory.CreateBuilder))]
        [Fact]
        public void Given_Services_When_CreateBuilder_Then_ReturnBuilderWithPropertyAutowiredEnable()
        {
            var services = new ServiceCollection();
            services.AddTransient<VestaService>();
            services.AddTransient<VestaServiceProperty>();
            var containerBuilder = new ContainerBuilder();

            var factory = new VestaAutofacServiceProviderFactory(containerBuilder);
            var builder = factory.CreateBuilder(services);

            builder.Should().Be(containerBuilder);
            var service = builder.Build().ResolveOptional<VestaService>();
            service?.Property.Should().NotBeNull();
        }

        [Trait("Category", VestaUnitTestCategories.DependencyInjection)]
        [Trait("Class", nameof(VestaAutofacServiceProviderFactory))]
        [Trait("Method", nameof(VestaAutofacServiceProviderFactory.CreateServiceProvider))]
        [Fact]
        public void Given_Services_When_CreateServiceProvider_Then_ReturnProvider()
        {
            var services = new ServiceCollection();
            services.AddTransient<VestaService>();
            var containerBuilder = new ContainerBuilder();

            var factory = new VestaAutofacServiceProviderFactory(containerBuilder);
            var provider = factory.CreateServiceProvider(containerBuilder);

            provider.Should().BeOfType<VestaAutofacServiceProvider>();
        }

        private class VestaService
        {
            public VestaServiceProperty Property { get; set; }
        }

        private class VestaServiceProperty
        {

        }


    }
}