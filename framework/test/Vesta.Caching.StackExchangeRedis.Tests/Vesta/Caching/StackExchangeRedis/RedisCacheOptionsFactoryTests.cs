using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IO;
using Vesta.TestBase;
using Xunit;

namespace Vesta.Caching.StackExchangeRedis
{
    public class RedisCacheOptionsFactoryTests
    {
        [Trait("Category", VestaUnitTestCategories.Caching)]
        [Trait("Class", nameof(RedisCacheOptionsFactory))]
        [Trait("Method", nameof(RedisCacheOptionsFactory.Create))]
        [Fact]
        public void Given_WithoutConfiguration_When_CreateRedisCacheOption_Then_ReturnEmptyOption()
        {
            var configuration = new Mock<IConfiguration>();
            var redisSection = new Mock<IConfigurationSection>();
            configuration.Setup(c => c.GetSection(It.IsAny<string>())).Returns(redisSection.Object);

            var factory = new RedisCacheOptionsFactory(configuration.Object);
            var options = factory.Create(null);

            options.Should().NotBeNull();

        }

        [Trait("Category", VestaUnitTestCategories.Caching)]
        [Trait("Class", nameof(RedisCacheOptionsFactory))]
        [Trait("Method", nameof(RedisCacheOptionsFactory.Create))]
        [Fact]
        public void Given_Configuration_When_CreateRedisCacheOption_Then_ReturnOption()
        {
            const string EQUAL_INSTANCE_NAME = "***instance***";
            const string EQUAL_CONFIGURATION = "***configuration***";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json")
                .Build();

            var factory = new RedisCacheOptionsFactory(configuration);
            var options = factory.Create(null);

            options.InstanceName.Should().Be(EQUAL_INSTANCE_NAME);
            options.Configuration.Should().Be(EQUAL_CONFIGURATION);

        }
    }
}