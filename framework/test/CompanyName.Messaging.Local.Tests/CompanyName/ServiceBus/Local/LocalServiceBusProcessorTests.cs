using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CompanyName.TestBase;
using Xunit;
using CompanyName.Messaging.InProcess;

namespace CompanyName.ServiceBus.Local
{
    public class LocalServiceBusProcessorTests
    {

        private readonly Mock<LocalServiceBusQueue> _queueStub;
        private readonly Mock<LocalServiceBusProcessor> _processorStub;

        public LocalServiceBusProcessorTests()
        {
            _queueStub = new Mock<LocalServiceBusQueue>();
            _processorStub = new Mock<LocalServiceBusProcessor>(_queueStub.Object)
            {
                CallBase = true
            };
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(LocalServiceBusProcessor))]
        [Trait("Method", nameof(LocalServiceBusProcessor.StartProcessingAsync))]
        [Fact]
        public void When_StartProcessing_Then_Started()
        {
            LocalServiceBusMessage message;

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            _queueStub.Setup(q => q.TryDequeue(out message)).Returns(true);

            var task = _processorStub.Object.StartProcessingAsync(cancellationToken);

            cancellationTokenSource.Cancel();

            _processorStub.Object.IsProcessing.Should().BeTrue();
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(LocalServiceBusProcessor))]
        [Trait("Method", nameof(LocalServiceBusProcessor.StartProcessingAsync))]
        [Fact]
        public void When_StartProcessingAndAnErrorOccursProcessingMessage_Then_ProcessErrorEventIsCalled()
        {
            LocalServiceBusMessage message;

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            _queueStub.Setup(q => q.TryDequeue(out message)).Throws<Exception>();
            _processorStub.Object.ProcessErrorAsync += ProcessErrorAsync;

            var task = _processorStub.Object.StartProcessingAsync(cancellationToken);

            Task ProcessErrorAsync(ProcessErrorEventArgs args)
            {
                Assert.True(true);

                return Task.CompletedTask;
            }
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(LocalServiceBusProcessor))]
        [Trait("Method", nameof(LocalServiceBusProcessor.StartProcessingAsync))]
        [Fact]
        public void When_StartProcessingAndProcessingMessage_Then_ProcessMessageEventIsCalled()
        {
            LocalServiceBusMessage message;

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            _queueStub.Setup(q => q.TryDequeue(out message)).Returns(true);
            _processorStub.Object.ProcessMessageAsync += ProcessMessageAsync;

            var task = _processorStub.Object.StartProcessingAsync(cancellationToken);

            Task ProcessMessageAsync(ProcessMessageEventArgs args)
            {
                Assert.True(true);

                return Task.CompletedTask;
            }
        }
    }
}
