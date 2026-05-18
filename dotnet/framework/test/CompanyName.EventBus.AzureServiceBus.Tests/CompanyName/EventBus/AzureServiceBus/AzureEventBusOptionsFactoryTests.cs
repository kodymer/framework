using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IO;
using CompanyName.TestBase;
using Xunit;

namespace CompanyName.EventBus.AzureServiceBus
{
    public class AzureEventBusOptionsFactoryTests
    {
        private readonly IConfigurationRoot _configuration;
        private readonly AzureServiceBusEventBusOptionsFactory _factory;

        public AzureEventBusOptionsFactoryTests()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json")
                .Build();

            _factory = new AzureServiceBusEventBusOptionsFactory(_configuration);
        }

        [Trait("Category", CompanyNameUnitTestCategories.EventBus)]
        [Trait("Class", nameof(AzureServiceBusEventBusOptionsFactory))]
        [Trait("Method", nameof(AzureServiceBusEventBusOptionsFactory.Create))]
        [Fact]
        public void Given_WithoutConfiguration_When_CreateAzureEventBusOption_Then_ReturnEmptyOption()
        {

            var options = _factory.Create(It.IsAny<string>());

            options.Should().NotBeNull();

        }

        [Trait("Category", CompanyNameUnitTestCategories.EventBus)]
        [Trait("Class", nameof(AzureServiceBusEventBusOptionsFactory))]
        [Trait("Method", nameof(AzureServiceBusEventBusOptionsFactory.Create))]
        [Fact]
        public void Given_Configuration_When_CreateAzureEventBusOption_Then_ReturnOptionWithConncetionStringPopulated()
        {
            const string EQUAL_CONNECTION_STRING = "***connection-string***";

            var options = _factory.Create(It.IsAny<string>());

            options.ConnectionString.Should().Be(EQUAL_CONNECTION_STRING);
        }

        [Trait("Category", CompanyNameUnitTestCategories.EventBus)]
        [Trait("Class", nameof(AzureServiceBusEventBusOptionsFactory))]
        [Trait("Method", nameof(AzureServiceBusEventBusOptionsFactory.Create))]
        [Fact]
        public void Given_Configuration_When_CreateAzureEventBusOption_Then_ReturnOptionWithTopicNamePopulated()
        {
            const string EQUAL_TOPIC_NAME = "***topic-name***";

            var options = _factory.Create(It.IsAny<string>());

            options.TopicName.Should().Be(EQUAL_TOPIC_NAME);
        }

        [Trait("Category", CompanyNameUnitTestCategories.EventBus)]
        [Trait("Class", nameof(AzureServiceBusEventBusOptionsFactory))]
        [Trait("Method", nameof(AzureServiceBusEventBusOptionsFactory.Create))]
        [Fact]
        public void Given_Configuration_When_CreateAzureEventBusOption_Then_ReturnOptionWithSubscriberNamePopulated()
        {
            const string EQUAL_TOPIC_NAME = "***subscriber-name***";

            var options = _factory.Create(It.IsAny<string>());

            options.SubscriberName.Should().Be(EQUAL_TOPIC_NAME);
        }
    }
}