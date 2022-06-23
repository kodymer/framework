using Azure.Messaging.ServiceBus;
using FluentAssertions;
using Moq;
using Vesta.TestBase;
using Vesta.TestBase.Fixtures;
using Xunit;

namespace Vesta.ServiceBus.Azure
{
    public class PublisherPoolTests : IClassFixture<ServiceRegistrarFixture>
    {
        private readonly ServiceRegistrarFixture _fixture;

        public PublisherPoolTests(ServiceRegistrarFixture fixture)
        {
            _fixture = fixture;
        }

        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(PublisherPool))]
        [Trait("Method", nameof(PublisherPool.GetSender))]
        [Fact]
        public void Given_ConnectionStringTopicNameAndSubscriberName_When_GetPublisher_Then_ReturnServiceBusPublisher()
        {
            const string topicName = "***topic-name***";
            const string connectionString = @$"Endpoint=sb://localhost/;
                SharedAccessKeyName=<redacted>;
                SharedAccessKey=<redacted>;
                EntityPath={topicName}";

            var serviceBusClient = new ServiceBusClient(connectionString);

            var connectionPoolStub = new Mock<IConnectionPool>();
            connectionPoolStub.Setup(c => c.GetClient(It.IsAny<string>()))
                .Returns(serviceBusClient);

            var publisherPoolStub = new Mock<PublisherPool>(connectionPoolStub.Object) { CallBase = true };
            publisherPoolStub.SetupGet(p => p.ServiceProvider)
                .Returns(_fixture.ServiceProvider);

            var publisher = publisherPoolStub.Object.GetSender(connectionString, topicName);
            publisher.Should().BeOfType<ServiceBusSender>();
        }
    }
}