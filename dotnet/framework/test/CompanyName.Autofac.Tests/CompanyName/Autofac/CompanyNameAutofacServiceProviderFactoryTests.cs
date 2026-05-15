using Autofac;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using CompanyName.Autofac.Extensions.DependencyInjection;
using System;
using CompanyName.TestBase;

namespace CompanyName.Autofac
{
    public class CompanyNameAutofacServiceProviderFactoryTests
    {
        [Trait("Category", CompanyNameUnitTestCategories.DependencyInjection)]
        [Trait("Class", nameof(CompanyNameAutofacServiceProviderFactory))]
        [Trait("Method", nameof(CompanyNameAutofacServiceProviderFactory.CreateBuilder))]
        [Fact]
        public void Given_Services_When_CreateBuilder_Then_ReturnBuilder()
        {
            var services = new ServiceCollection();
            services.AddTransient<CompanyNameService>();
            var containerBuilder = new ContainerBuilder();

            var factory = new CompanyNameAutofacServiceProviderFactory(containerBuilder);
            var builder = factory.CreateBuilder(services);

            builder.Should().Be(containerBuilder);
            builder.Build().ResolveOptional<CompanyNameService>().Should().NotBeNull();
        }

        [Trait("Category", CompanyNameUnitTestCategories.DependencyInjection)]
        [Trait("Class", nameof(CompanyNameAutofacServiceProviderFactory))]
        [Trait("Method", nameof(CompanyNameAutofacServiceProviderFactory.CreateBuilder))]
        [Fact]
        public void Given_Null_When_CreateBuilder_Then_ThrowArgumentError()
        {
            var containerBuilder = new ContainerBuilder();

            var factory = new CompanyNameAutofacServiceProviderFactory(containerBuilder);
            var action = () => factory.CreateBuilder(null);

            action.Should().ThrowExactly<ArgumentNullException>();
        }


        [Trait("Category", CompanyNameUnitTestCategories.DependencyInjection)]
        [Trait("Class", nameof(CompanyNameAutofacServiceProviderFactory))]
        [Trait("Method", nameof(CompanyNameAutofacServiceProviderFactory.CreateBuilder))]
        [Fact]
        public void Given_Services_When_CreateBuilder_Then_ReturnBuilderWithPropertyAutowiredEnable()
        {
            var services = new ServiceCollection();
            services.AddTransient<CompanyNameService>();
            services.AddTransient<CompanyNameServiceProperty>();
            var containerBuilder = new ContainerBuilder();

            var factory = new CompanyNameAutofacServiceProviderFactory(containerBuilder);
            var builder = factory.CreateBuilder(services);

            builder.Should().Be(containerBuilder);
            var service = builder.Build().ResolveOptional<CompanyNameService>();
            service?.Property.Should().NotBeNull();
        }

        [Trait("Category", CompanyNameUnitTestCategories.DependencyInjection)]
        [Trait("Class", nameof(CompanyNameAutofacServiceProviderFactory))]
        [Trait("Method", nameof(CompanyNameAutofacServiceProviderFactory.CreateServiceProvider))]
        [Fact]
        public void Given_Services_When_CreateServiceProvider_Then_ReturnProvider()
        {
            var services = new ServiceCollection();
            services.AddTransient<CompanyNameService>();
            var containerBuilder = new ContainerBuilder();

            var factory = new CompanyNameAutofacServiceProviderFactory(containerBuilder);
            var provider = factory.CreateServiceProvider(containerBuilder);

            provider.Should().BeOfType<CompanyNameAutofacServiceProvider>();
        }

        private class CompanyNameService
        {
            public CompanyNameServiceProperty Property { get; set; }
        }

        private class CompanyNameServiceProperty
        {

        }


    }
}