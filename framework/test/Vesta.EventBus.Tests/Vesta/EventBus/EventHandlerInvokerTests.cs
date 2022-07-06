using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Vesta.EventBus.Abstracts;
using Vesta.TestBase;
using Xunit;

namespace Vesta.EventBus
{
    public class EventHandlerInvokerTests
    {
        private readonly Mock<EventHandlerInvoker> _invoker;

        public EventHandlerInvokerTests()
        {
            _invoker = new Mock<EventHandlerInvoker>()
            {
                CallBase = true
            };
        }

        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(EventHandlerInvoker))]
        [Trait("Method", nameof(EventHandlerInvoker.InvokeAsync))]
        [Fact]
        public async Task Given_IntegrationEventHandlerAndEventTypeAndEventData_When_HandlerInvoked_Then_ExecuteHandler()
        {
            var handler = new VestaIntegrationEventHandler();
            var eventType = typeof(VestaEto);
            var eventData = new VestaEto();

            await _invoker.Object.InvokeAsync(handler, eventType, eventData);
        }

        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(EventHandlerInvoker))]
        [Trait("Method", nameof(EventHandlerInvoker.InvokeAsync))]
        [Fact]
        public async Task Given_DomainEventHandlerAndEventTypeAndEventData_When_HandlerInvoked_Then_ExecuteHandler()
        {
            var handler = new VestaDomainEventHandler();
            var eventType = typeof(VestaEto);
            var eventData = new VestaEto();

            await _invoker.Object.InvokeAsync(handler, eventType, eventData);

        }

        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(EventHandlerInvoker))]
        [Trait("Method", nameof(EventHandlerInvoker.InvokeAsync))]
        [Fact]
        public void Given_IntegrationEventHandlerAndEventTypeAndEventData_When_HandlerInvoked_Then_ThrowNotSupportError()
        {
            var handler = new BadImplVestaHandler();
            var eventType = typeof(VestaEto);
            var eventData = new VestaEto();

            var action = async () => await _invoker.Object.InvokeAsync(handler, eventType, eventData);

            action.Should().ThrowExactlyAsync<NotSupportedException>();
        }

        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(EventHandlerInvoker))]
        [Trait("Method", nameof(EventHandlerInvoker.InvokeAsync))]
        [Fact]
        public void Given_IntegrationEventHandler_When_HandlerInvoked_Then_ThrowArgumentError()
        {
            var handler = new VestaIntegrationEventHandler();

            var action = async () => await _invoker.Object.InvokeAsync(handler, null, null);

            action.Should().ThrowExactlyAsync<ArgumentNullException>();

        }

        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(EventHandlerInvoker))]
        [Trait("Method", nameof(EventHandlerInvoker.InvokeAsync))]
        [Fact]
        public void When_HandlerInvoked_Then_ThrowArgumentError()
        {
            var invoker = new Mock<EventHandlerInvoker>();
            var action = async () => await _invoker.Object.InvokeAsync(null, null, null);

            action.Should().ThrowExactlyAsync<ArgumentNullException>();
        }


        private class BadImplVestaHandler : IEventHandler
        {

        }

        private class VestaIntegrationEventHandler : IIntegrationEventHandler<VestaEto>
        {
            public Task HandleEventAsync(VestaEto args)
            {
                return Task.CompletedTask;
            }
        }

        private class VestaDomainEventHandler : IDomainEventHandler<VestaEto>
        {
            public Task HandleEventAsync(VestaEto args)
            {
                return Task.CompletedTask;
            }
        }

        private class VestaEto
        {

        } 
    }
}