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
        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(EventHandlerInvoker))]
        [Trait("Method", nameof(EventHandlerInvoker.InvokeAsync))]
        [Fact]
        public async Task Given_HandlerEventTypeAndEventData_When_HandlerInvoked_Then_ExecuteHandler()
        {
            var handler = new VestaHandler();
            var eventType = typeof(VestaEto);
            var eventData = new VestaEto();

            var invoker = new Mock<EventHandlerInvoker>();
            await invoker.Object.InvokeAsync(handler, eventType, eventData);

        }

        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(EventHandlerInvoker))]
        [Trait("Method", nameof(EventHandlerInvoker.InvokeAsync))]
        [Fact]
        public void Given_HandlerEventTypeAndEventData_When_HandlerInvoked_Then_ThrowNotSupportError()
        {
            var handler = new VestaHandler();
            var eventType = typeof(VestaEto);
            var eventData = new VestaEto();

            var invoker = new Mock<EventHandlerInvoker>();
            var action = async () => await invoker.Object.InvokeAsync(handler, eventType, eventData);

            action.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(EventHandlerInvoker))]
        [Trait("Method", nameof(EventHandlerInvoker.InvokeAsync))]
        [Fact]
        public void Given_Handler_When_HandlerInvoked_Then_ThrowArgumentError()
        {
            var handler = new VestaHandler();

            var invoker = new Mock<EventHandlerInvoker>();
            var action = async () => await invoker.Object.InvokeAsync(handler, null, null);

            action.Should().ThrowExactlyAsync<ArgumentNullException>();

        }

        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(EventHandlerInvoker))]
        [Trait("Method", nameof(EventHandlerInvoker.InvokeAsync))]
        [Fact]
        public void When_HandlerInvoked_Then_ThrowArgumentError()
        {
            var invoker = new Mock<EventHandlerInvoker>();
            var action = async () => await invoker.Object.InvokeAsync(null, null, null);

            action.Should().ThrowExactlyAsync<ArgumentNullException>();
        }


        private class BadImplVestaHandler : IEventHandler
        {

        }

        private class VestaHandler : IIntegrationEventHandler<VestaEto>
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