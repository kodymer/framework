using FluentAssertions;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.ServiceBus.Azure;
using Vesta.TestBase;
using Xunit;

namespace Vesta.EventBus.Azure
{
    public class AzureServiceBusMessageConsumerTests
    {
        private readonly Mock<AzureServiceBusMessageConsumer> consumer;

        public AzureServiceBusMessageConsumerTests()
        {
            var processorStub = new Mock<IProcessorPool>();
            consumer = new Mock<AzureServiceBusMessageConsumer>(processorStub.Object)
            {
                CallBase = true
            };
        }

        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(AzureServiceBusMessageConsumer))]
        [Trait("Method", nameof(AzureServiceBusMessageConsumer.Initialize))]
        [Fact]
        public void Given_ConnectionStringTopicNameAndSubscriberName_When_Initialize_Then_Initialized()
        {
            const string topicName = "***topic-name***";
            const string subscriberName = "***subscriber-name***";
            const string connectionString = "***connection-string***";

            consumer.Protected().Setup("StartProcessing");

            consumer.Object.Initialize(connectionString, topicName, subscriberName);
        }

        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(AzureServiceBusMessageConsumer))]
        [Trait("Method", nameof(AzureServiceBusMessageConsumer.Initialize))]
        [Fact]
        public void Given_ConnectionStringAndTopicName_When_Initialize_Then_ThrowArgumentError()
        {

            const string topicName = "***topic-name***";
            const string connectionString = "***connection-string***";

            var action = () => consumer.Object.Initialize(connectionString, topicName, null);

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(AzureServiceBusMessageConsumer))]
        [Trait("Method", nameof(AzureServiceBusMessageConsumer.Initialize))]
        [Fact]
        public void Given_ConnectionString_When_Initialize_Then_ThrowArgumentError()
        {
            const string connectionString = "***connection-string***";

            var action = () => consumer.Object.Initialize(connectionString, null, null);

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(AzureServiceBusMessageConsumer))]
        [Trait("Method", nameof(AzureServiceBusMessageConsumer.Initialize))]
        [Fact]
        public void When_Initialized_Then_ThrowArgumentError()
        {
            var action = () => consumer.Object.Initialize(null, null, null);

            action.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
