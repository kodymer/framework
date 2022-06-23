using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IO;
using Vesta.TestBase;
using Xunit;

namespace Vesta.Dapper
{
    public class DatabaseOptionsFactoryTests
    {
        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(DatabaseOptionsFactory))]
        [Trait("Method", nameof(DatabaseOptionsFactory.Create))]
        [Fact]
        public void Given_WithoutConfiguration_When_CreateDatabaseOption_Then_ReturnEmptyOption()
        {
            var configuration = new Mock<IConfiguration>();
            var redisSection = new Mock<IConfigurationSection>();
            configuration.Setup(c => c.GetSection(It.IsAny<string>())).Returns(redisSection.Object);

            var factory = new DatabaseOptionsFactory(configuration.Object);
            var options = factory.Create(null);

            options.Should().NotBeNull();

        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(DatabaseOptionsFactory))]
        [Trait("Method", nameof(DatabaseOptionsFactory.Create))]
        [Fact]
        public void Given_Configuration_When_CreateDatabaseOption_Then_ReturnOptionWithConnectionStringPopulated()
        {
            const string EQUAL_CONNECTION_STRING = "***connection-string***";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json")
                .Build();

            var factory = new DatabaseOptionsFactory(configuration);
            var options = factory.Create(null);

            options.ConnectionString.Should().Be(EQUAL_CONNECTION_STRING);
        }
    }
}