using Azure.Messaging.ServiceBus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using Vesta.TestBase;
using Xunit;
using Vesta.Autofac;
using Autofac;
using Vesta.Autofac.Extensions.DependencyInjection;
using Vesta.TestBase.Fixtures;

namespace Vesta.ServiceBus.Azure
{
    public class ProccessorPoolTests : IClassFixture<ServiceRegistrarFixture>
    {
        private readonly ServiceRegistrarFixture _fixture;

        public ProccessorPoolTests(ServiceRegistrarFixture fixture)
        {
            _fixture = fixture;
        }

        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(ProcessorPool))]
        [Trait("Method", nameof(ProcessorPool.GetProcessor))]
        [Fact]
        public void Given_ConnectionStringTopicNameAndSubscriberName_When_GetProcessor_Then_ReturnServiceBusProcessor()
        {
            const string topicName = "***topic-name***";
            const string subscriberName = "***subscriber-name***";
            const string connectionString = @$"Endpoint=sb://localhost/;
                SharedAccessKeyName=<redacted>;
                SharedAccessKey=<redacted>;
                EntityPath={topicName}";

            var options = Options.Create(new ServiceBusProcessorOptions());
            var serviceBusClient = new ServiceBusClient(connectionString);

            var connectionPoolStub = new Mock<IConnectionPool>();
            connectionPoolStub.Setup(c => c.GetClient(It.IsAny<string>()))
                .Returns(serviceBusClient);

            var processorPoolStub = new Mock<ProcessorPool>(options, connectionPoolStub.Object) { CallBase = true };
            processorPoolStub.SetupGet(p => p.ServiceProvider)
                .Returns(_fixture.ServiceProvider);

            var processor = processorPoolStub.Object.GetProcessor(connectionString, topicName, subscriberName);
            processor.Should().BeOfType<ServiceBusProcessor>();
        }
    }
}