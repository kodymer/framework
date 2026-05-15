using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;
using CompanyName.TestBase;
using CompanyName.TestBase.Fixtures;
using Xunit;
using CompanyName.Messaging.InProcess;

namespace CompanyName.EventBus
{
    public class LocalEventBusTests : IClassFixture<ServiceRegistrarFixture>
    {
        private readonly ServiceRegistrarFixture _fixture;

        public LocalEventBusTests(ServiceRegistrarFixture fixture)
        {
            _fixture = fixture;
        }

        [Trait("Category", CompanyNameUnitTestCategories.EventBus)]
        [Trait("Class", nameof(LocalEventBus))]
        [Trait("Method", nameof(LocalEventBus.PublishAsync))]
        [Fact]
        public async Task Given_EventTransferObject_When_MessageIsPosted_Then_ItIsPostedSuccessfully()
        {

            var eventHandlerOptions = Options.Create(new EventHandlerOptions());
            var eventHandlerTypeProvider = new Mock<DomainEventHandlerTypeProvider>(eventHandlerOptions);
            var consumer = new Mock<ILocalServiceBusMessageConsumer>();
            var publisher = new Mock<ILocalServiceBusSender>();
            var eventHandlerInvoker = new Mock<IEventHandlerInvoker>();

            var eventBusStub = new Mock<LocalEventBus>(
                _fixture.ServiceProvider, eventHandlerTypeProvider.Object,
                consumer.Object, publisher.Object, eventHandlerInvoker.Object);

            await eventBusStub.Object.PublishAsync(new CompanyNameEto());
        }

        private class CompanyNameEto
        {
            public string Property { get; set; }
        }

    }
}
