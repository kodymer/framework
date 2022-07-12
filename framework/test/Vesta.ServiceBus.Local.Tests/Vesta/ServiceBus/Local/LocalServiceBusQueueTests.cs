using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.TestBase;
using Xunit;

namespace Vesta.ServiceBus.Local
{
    public class LocalServiceBusQueueTests
    {

        private readonly Mock<LocalServiceBusQueue> _queueStub;

        public LocalServiceBusQueueTests()
        {
            _queueStub = new Mock<LocalServiceBusQueue>() { 
                CallBase = true 
            };
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(LocalServiceBusQueue))]
        [Trait("Method", nameof(LocalServiceBusQueue.Enqueue))]
        [Fact]
        public void Given_LocalMessage_When_Enqueue_Then_ItIsPlacedInTheQueue()
        {
            var message = new LocalServiceBusMessage();

            _queueStub.Object.Enqueue(message);

        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(LocalServiceBusQueue))]
        [Trait("Method", nameof(LocalServiceBusQueue.Enqueue))]
        [Fact]
        public void Given_LocalMessage_When_Enqueue_Then_MessageReceivedEventIsCalled()
        {
            var message = new LocalServiceBusMessage();

            _queueStub.Object.MessageReceived += MessageReceived;

            _queueStub.Object.Enqueue(message);

            void MessageReceived(object sender, MessageReceivedEventArgs args)
            {
                Assert.True(true);
            }

        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(LocalServiceBusQueue))]
        [Trait("Method", nameof(LocalServiceBusQueue.Enqueue))]
        [Fact]
        public void Given_LocalMessage_When_Enqueue_Then_ChangedEventIsCalled()
        {
            var message = new LocalServiceBusMessage();

            _queueStub.Object.Changed += Changed;

            _queueStub.Object.Enqueue(message);

            void Changed(object sender, ChangedEventArgs args)
            {
                Assert.True(true);
            }

        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(LocalServiceBusQueue))]
        [Trait("Method", nameof(LocalServiceBusQueue.TryDequeue))]
        [Fact]
        public void When_TryDequeue_Then_LocalMessageIsGot()
        {
            LocalServiceBusMessage messageMock;

            _queueStub.Object.Enqueue(new LocalServiceBusMessage());
            _queueStub.Object.TryDequeue(out messageMock);

            messageMock.Should().NotBeNull();
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(LocalServiceBusQueue))]
        [Trait("Method", nameof(LocalServiceBusQueue.Enqueue))]
        [Fact]
        public void When_TryDequeue_Then_MessageSentEventIsCalled()
        {
            LocalServiceBusMessage message;

            _queueStub.Object.MessageSent += MessageSent;

            _queueStub.Object.TryDequeue(out message);

            void MessageSent(object sender, MessageSentEventArgs args)
            {
                Assert.True(true);
            }

        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(LocalServiceBusQueue))]
        [Trait("Method", nameof(LocalServiceBusQueue.Enqueue))]
        [Fact]
        public void When_TryDequeue_Then_ChangedEventIsCalled()
        {
            LocalServiceBusMessage message;

            _queueStub.Object.Changed += Changed;

            _queueStub.Object.TryDequeue(out message);

            void Changed(object sender, ChangedEventArgs args)
            {
                Assert.True(true);
            }

        }
    }
}
