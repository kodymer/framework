using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Vesta.TestBase;
using Xunit;

namespace Vesta.ServiceBus.Local
{
    public class LocalServiceBusSenderTests
    {
        private readonly Mock<LocalServiceBusQueue> _queue;
        private readonly Mock<LocalServiceBusSender> _sender;

        public LocalServiceBusSenderTests()
        {
            _queue = new Mock<LocalServiceBusQueue>();
            _sender = new Mock<LocalServiceBusSender>(_queue.Object)
            {
                CallBase = true
            };
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(LocalServiceBusSender))]
        [Trait("Method", nameof(LocalServiceBusSender.SendMessageAsync))]
        [Fact]
        public async Task Given_Message_When_SendMessage_Then_ItIsQueued()
        {
            var message = new LocalServiceBusMessage();

            _queue.Setup(q => q.Enqueue(It.IsAny<LocalServiceBusMessage>()));

            await _sender.Object.SendMessageAsync(message);
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(LocalServiceBusSender))]
        [Trait("Method", nameof(LocalServiceBusSender.SendMessageAsync))]
        [Fact]
        public void Given_Null_When_SendMessage_Then_ThrowArgumentError()
        {
            var action = async () => await _sender.Object.SendMessageAsync(null);

            action.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
    }
}