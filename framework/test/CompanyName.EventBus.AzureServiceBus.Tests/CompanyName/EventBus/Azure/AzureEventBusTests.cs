using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyName.Security.Claims;
using CompanyName.TestBase;
using CompanyName.TestBase.Fixtures;
using Xunit;
using CompanyName.Messaging.AzureServiceBus;
using CompanyName.EventBus.AzureServiceBus;

namespace CompanyName.EventBus.Azure
{
    public class AzureEventBusTests : IClassFixture<ServiceRegistrarFixture>
    {
        private readonly ServiceRegistrarFixture _fixture;

        public AzureEventBusTests(ServiceRegistrarFixture fixture)
        {
            _fixture = fixture;
        }

        [Trait("Category", CompanyNameUnitTestCategories.EventBus)]
        [Trait("Class", nameof(AzureServiceBusEventBus))]
        [Trait("Method", nameof(AzureServiceBusEventBus.PublishAsync))]
        [Fact]
        public async Task Given_EventTransferObject_When_MessageIsPosted_Then_ItIsPostedSuccessfully()
        {

            var azureEventBusOptions = Options.Create(new AzureServiceBusEventBusOptions());
            var currentPrincipalAccessor = new Mock<ICurrentPrincipalAccessor>();
            var eventHandlerOptions = Options.Create(new EventHandlerOptions());
            var eventHandlerTypeProvider = new Mock<IntegrationEventHandlerTypeProvider>(eventHandlerOptions);
            var consumer = new Mock<IAzureServiceBusMessageConsumer>();
            var publisher = new Mock<IPublisherPool>();
            var eventHandlerInvoker = new Mock<IEventHandlerInvoker>();

            var eventBusStub = new Mock<AzureServiceBusEventBus>(
                _fixture.ServiceProvider, currentPrincipalAccessor.Object, azureEventBusOptions, eventHandlerTypeProvider.Object,
                consumer.Object, publisher.Object, eventHandlerInvoker.Object);

            await eventBusStub.Object.PublishAsync(new CompanyNameEto());

        }

        private class CompanyNameEto
        {
            public string Property { get; set; }
        }

    }
}
