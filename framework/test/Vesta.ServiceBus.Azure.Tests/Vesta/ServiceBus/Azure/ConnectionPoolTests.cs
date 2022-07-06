using Azure.Messaging.ServiceBus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using Vesta.TestBase;
using Vesta.TestBase.Fixtures;
using Xunit;

namespace Vesta.ServiceBus.Azure
{
    public class ConnectionPoolTests : IClassFixture<ServiceRegistrarFixture>
    {
        private Mock<ConnectionPool> _connectionPoolStub;

        public ConnectionPoolTests(ServiceRegistrarFixture fixture)
        {
            var options = Options.Create(new ServiceBusClientOptions());
            _connectionPoolStub = new Mock<ConnectionPool>(options)
            {
                CallBase = true
            };
            _connectionPoolStub.SetupGet(c => c.ServiceProvider)
                .Returns(fixture.ServiceProvider);
        }

        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(ConnectionPool))]
        [Trait("Method", nameof(ConnectionPool.GetClient))]
        [Fact]
        public void Given_ConnectionString_When_GetClient_Then_ReturnServiceBusClient()
        {
            const string ConnectionString = @"Endpoint=sb://localhost/;
                  SharedAccessKeyName=<redacted>;
                  SharedAccessKey=<redacted>;
                  EntityPath=default";

            var client = _connectionPoolStub.Object.GetClient(ConnectionString);

            client.Should().NotBeNull();
            client.Should().BeOfType<ServiceBusClient>();
        }

        [Trait("Category", VestaUnitTestCategories.EventBus)]
        [Trait("Class", nameof(ConnectionPool))]
        [Trait("Method", nameof(ConnectionPool.GetClient))]
        [Fact]
        public void Given_NotValidConnectionString_When_GetClient_Then_ThrowFormatError()
        {
            const string ConnectionString = "***connection-string***";

            var action = () => _connectionPoolStub.Object.GetClient(ConnectionString);

            action.Should().ThrowExactly<FormatException>();
        }
    }
}