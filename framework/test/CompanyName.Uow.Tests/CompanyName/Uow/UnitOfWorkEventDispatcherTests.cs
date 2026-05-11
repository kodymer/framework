using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CompanyName.EventBus.Abstractions;
using CompanyName.TestBase;
using CompanyName.Uow.Fixtures;
using Xunit;

namespace CompanyName.Uow
{

    public class UnitOfWorkEventDispatcherTests : IClassFixture<UnitOfWorkServiceRegistrarFixture>
    {
        private readonly Mock<IEventStore> _storeStub;
        private readonly Mock<UnitOfWorkEventDispatcher> _managerStub;
        private readonly UnitOfWorkServiceRegistrarFixture _fixture;

        public UnitOfWorkEventDispatcherTests(UnitOfWorkServiceRegistrarFixture fixture)
        {
            _fixture = fixture;

            _storeStub = new Mock<IEventStore>();
            _managerStub = new Mock<UnitOfWorkEventDispatcher>(_storeStub.Object)
            {
                CallBase = true
            };
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventDispatcher))]
        [Trait("Method", nameof(UnitOfWorkEventDispatcher.CreateAndInsertAsync))]
        [Fact]
        public async Task Given_PublisherAndEventRecordAndPriority_When_CreateAndInsertPublishing_Then_ResturnPublishing()
        {
            var publisher = _fixture.ServiceProvider.GetService<ILocalEventBus>();
            var eventRecord = new UnitOfWorkEventRecord(new(), new());
            var cutomPriority = 3;

            _managerStub.Setup(m => m.InsertAsync(It.IsAny<UnitOfWorkEventPublishing>(), It.IsAny<CancellationToken>()));

            var newPublishing = await _managerStub.Object.CreateAndInsertAsync(publisher, eventRecord, cutomPriority);

            newPublishing.Should().NotBeNull().And.Subject
                .Should().BeOfType<UnitOfWorkEventPublishing>();
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventDispatcher))]
        [Trait("Method", nameof(UnitOfWorkEventDispatcher.CreateAndInsertAsync))]
        [Fact]
        public async Task Given_PublisherAndEventRecordWithoutPriority_When_CreateAndInsertPublishing_Then_ResturnPublishing()
        {
            const long EQUAL_PRIORITY = 3;
            var publisher = _fixture.ServiceProvider.GetService<ILocalEventBus>();
            var eventRecord = new UnitOfWorkEventRecord(new(), new());
            var unorderedItems = new List<UnitOfWorkEventPublishing> {
                new UnitOfWorkEventPublishing(It.IsAny<IEventBus>(), It.IsAny<UnitOfWorkEventRecord>(), 1),
                new UnitOfWorkEventPublishing(It.IsAny<IEventBus>(), It.IsAny<UnitOfWorkEventRecord>(), 2),
            };

            _storeStub.Setup(s => s.Get(null)).Returns(unorderedItems);

            _managerStub.Setup(m => m.InsertAsync(It.IsAny<UnitOfWorkEventPublishing>(), It.IsAny<CancellationToken>()));

            var newPublishing = await _managerStub.Object.CreateAndInsertAsync(publisher, eventRecord);

            newPublishing.Should().NotBeNull().And.Subject
                .Should().BeOfType<UnitOfWorkEventPublishing>().And.Subject
                .Should().Match((UnitOfWorkEventPublishing e) => e.Priority.Equals(EQUAL_PRIORITY));
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventDispatcher))]
        [Trait("Method", nameof(UnitOfWorkEventDispatcher.CreateAndInsertAsync))]
        [Fact]
        public void Given_Publisher_When_CreateAndInsertPublishing_Then_ThrowArgumentError()
        {
            var publisher = _fixture.ServiceProvider.GetService<ILocalEventBus>();
            var action = async () => await _managerStub.Object.CreateAndInsertAsync(publisher, null);

            action.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventDispatcher))]
        [Trait("Method", nameof(UnitOfWorkEventDispatcher.CreateAndInsertAsync))]
        [Fact]
        public void Given_Null_When_CreateAndInsertPublishing_Then_ThrowArgumentError()
        {
            var action = async () => await _managerStub.Object.CreateAndInsertAsync(null, null);

            action.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventDispatcher))]
        [Trait("Method", nameof(UnitOfWorkEventDispatcher.InsertAsync))]
        [Fact]
        public async Task Given_Publishing_When_InsertPublishing_Then_Successful()
        {
            var publisher = _fixture.ServiceProvider.GetService<ILocalEventBus>();
            var eventRecord = new UnitOfWorkEventRecord(new(), new());
            var publishing = new UnitOfWorkEventPublishing(It.IsAny<IEventBus>(), It.IsAny<UnitOfWorkEventRecord>(), 1);

            _storeStub.Setup(m => m.PushAsync(It.IsAny<UnitOfWorkEventPublishing>(), It.IsAny<CancellationToken>()));

            await _managerStub.Object.InsertAsync(publishing, It.IsAny<CancellationToken>());

        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventDispatcher))]
        [Trait("Method", nameof(UnitOfWorkEventDispatcher.InsertAsync))]
        [Fact]
        public void Given_Null_When_InsertPublishing_Then_Successful()
        {
            var action = async () => await _managerStub.Object.InsertAsync(null, It.IsAny<CancellationToken>());

            action.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventDispatcher))]
        [Trait("Method", nameof(UnitOfWorkEventDispatcher.PublishAllAsync))]
        [Fact]
        public async Task When_PublishAllPublishing_Then_Sucessful()
        {
            var publisher = _fixture.ServiceProvider.GetService<ILocalEventBus>();
            var eventRecord = new UnitOfWorkEventRecord(new(), new());
            var unorderedItems = new List<UnitOfWorkEventPublishing> {
                new UnitOfWorkEventPublishing(It.IsAny<IEventBus>(), It.IsAny<UnitOfWorkEventRecord>(), 1),
                new UnitOfWorkEventPublishing(It.IsAny<IEventBus>(), It.IsAny<UnitOfWorkEventRecord>(), 2),
            };
            var publishing = new UnitOfWorkEventPublishing(publisher, eventRecord, 3);

            _storeStub.Setup(s => s.Get(null)).Returns(unorderedItems);
            _storeStub.Setup(s => s.PopAsync(It.IsAny<CancellationToken>())).ReturnsAsync(publishing);

            await _managerStub.Object.PublishAllAsync();
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventDispatcher))]
        [Trait("Method", nameof(UnitOfWorkEventDispatcher.PublishAllAsync))]
        [Fact]
        public async Task Given_ThereAreNotRegisteredItems_When_PublishAllPublishing_Then_Successful()
        {
            var unorderedItems = new List<UnitOfWorkEventPublishing>();

            _storeStub.Setup(s => s.Get(null)).Returns(unorderedItems);

            await _managerStub.Object.PublishAllAsync();
        }
    }
}
