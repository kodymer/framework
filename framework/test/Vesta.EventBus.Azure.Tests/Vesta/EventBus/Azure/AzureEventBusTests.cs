using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.ServiceBus.Azure;
using Vesta.TestBase;
using Vesta.TestBase.Fixtures;
using Xunit;

namespace Vesta.EventBus.Azure
{
    public class AzureEventBusTests : IClassFixture<ServiceRegistrarFixture>
    {
        private readonly ServiceRegistrarFixture _fixture;

        public AzureEventBusTests(ServiceRegistrarFixture fixture)
        {
            _fixture = fixture;
        }

        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(AzureEventBus))]
        [Trait("Method", nameof(AzureEventBus.PublishAsync))]
        [Fact]
        public async Task Given_EventTransferObject_When_MessageIsPosted_Then_ItIsPostedSuccessfully()
        {

            var azureEventBusOptions = Options.Create(new AzureEventBusOptions());
            var eventHandlerOptions = Options.Create(new EventHandlerOptions());
            var eventHandlerTypeProvider = new Mock<IntegrationEventHandlerTypeProvider>(eventHandlerOptions);
            var consumer = new Mock<IAzureServiceBusMessageConsumer>();
            var publisher = new Mock<IPublisherPool>();
            var eventHandlerInvoker = new Mock<IEventHandlerInvoker>();

            var eventBusStub = new Mock<AzureEventBus>(
                _fixture.ServiceProvider, azureEventBusOptions, eventHandlerTypeProvider.Object,
                consumer.Object, publisher.Object, eventHandlerInvoker.Object);

            await eventBusStub.Object.PublishAsync(new VestaEto());

        }

        private class VestaEto
        {
            public string Property { get; set; }
        }

    }
}
