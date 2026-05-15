using CompanyName.EventBus.Abstractions;
using CompanyName.TestBase;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CompanyName.EventBus
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

        [Trait("Category", CompanyNameUnitTestCategories.EventBus)]
        [Trait("Class", nameof(EventHandlerInvoker))]
        [Trait("Method", nameof(EventHandlerInvoker.InvokeAsync))]
        [Fact]
        public async Task Given_IntegrationEventHandlerAndEventTypeAndEventData_When_HandlerInvoked_Then_ExecuteHandler()
        {
            var handler = new CompanyNameIntegrationEventHandler();
            var eventType = typeof(CompanyNameEto);
            var eventData = new CompanyNameEto();

            await _invoker.Object.InvokeAsync(handler, eventType, eventData);
        }

        [Trait("Category", CompanyNameUnitTestCategories.EventBus)]
        [Trait("Class", nameof(EventHandlerInvoker))]
        [Trait("Method", nameof(EventHandlerInvoker.InvokeAsync))]
        [Fact]
        public async Task Given_DomainEventHandlerAndEventTypeAndEventData_When_HandlerInvoked_Then_ExecuteHandler()
        {
            var handler = new CompanyNameDomainEventHandler();
            var eventType = typeof(CompanyNameEto);
            var eventData = new CompanyNameEto();

            await _invoker.Object.InvokeAsync(handler, eventType, eventData);

        }

        [Trait("Category", CompanyNameUnitTestCategories.EventBus)]
        [Trait("Class", nameof(EventHandlerInvoker))]
        [Trait("Method", nameof(EventHandlerInvoker.InvokeAsync))]
        [Fact]
        public void Given_IntegrationEventHandlerAndEventTypeAndEventData_When_HandlerInvoked_Then_ThrowNotSupportError()
        {
            var handler = new BadImplCompanyNameHandler();
            var eventType = typeof(CompanyNameEto);
            var eventData = new CompanyNameEto();

            var action = async () => await _invoker.Object.InvokeAsync(handler, eventType, eventData);

            action.Should().ThrowExactlyAsync<NotSupportedException>();
        }

        [Trait("Category", CompanyNameUnitTestCategories.EventBus)]
        [Trait("Class", nameof(EventHandlerInvoker))]
        [Trait("Method", nameof(EventHandlerInvoker.InvokeAsync))]
        [Fact]
        public void Given_IntegrationEventHandler_When_HandlerInvoked_Then_ThrowArgumentError()
        {
            var handler = new CompanyNameIntegrationEventHandler();

            var action = async () => await _invoker.Object.InvokeAsync(handler, null, null);

            action.Should().ThrowExactlyAsync<ArgumentNullException>();

        }

        [Trait("Category", CompanyNameUnitTestCategories.EventBus)]
        [Trait("Class", nameof(EventHandlerInvoker))]
        [Trait("Method", nameof(EventHandlerInvoker.InvokeAsync))]
        [Fact]
        public void When_HandlerInvoked_Then_ThrowArgumentError()
        {
            var invoker = new Mock<EventHandlerInvoker>();
            var action = async () => await _invoker.Object.InvokeAsync(null, null, null);

            action.Should().ThrowExactlyAsync<ArgumentNullException>();
        }


        private class BadImplCompanyNameHandler : IEventHandler
        {

        }

        private class CompanyNameIntegrationEventHandler : IEventHandler<CompanyNameEto>
        {
            public Task HandleAsync(CompanyNameEto args, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }
        }

        private class CompanyNameDomainEventHandler : IEventHandler<CompanyNameEto>
        {
            public Task HandleAsync(CompanyNameEto args, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }
        }

        private class CompanyNameEto
        {

        } 
    }
}