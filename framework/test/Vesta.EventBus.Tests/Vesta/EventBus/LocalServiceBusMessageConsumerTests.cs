using FluentAssertions;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.ServiceBus.Local;
using Vesta.TestBase;
using Xunit;

namespace Vesta.EventBus.Azure
{
    public class LocalServiceBusMessageConsumerTests
    {
        private readonly Mock<LocalServiceBusMessageConsumer> consumer;

        public LocalServiceBusMessageConsumerTests()
        {
            var processorStub = new Mock<ILocalServiceBusProcessor>();
            consumer = new Mock<LocalServiceBusMessageConsumer>(processorStub.Object)
            {
                CallBase = true
            };
        }

        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(LocalServiceBusMessageConsumer))]
        [Trait("Method", nameof(LocalServiceBusMessageConsumer.Initialize))]
        [Fact]
        public void When_Initialize_Then_Initialized()
        {

            consumer.Protected().Setup("StartProcessing");

            consumer.Object.Initialize();
        }

    }
}
