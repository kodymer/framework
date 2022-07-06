using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vesta.EventBus.Abstracts;
using Vesta.TestBase;
using Vesta.Uow.Fixtures;
using Xunit;

namespace Vesta.Uow
{

    public class UnitOfWorkEventPublishingManagerTests : IClassFixture<UnitOfWorkServiceRegistrarFixture>
    {
        private readonly Mock<IUnitOfWorkEventPublishingStore> _storeStub;
        private readonly Mock<UnitOfWorkEventPublishingManager> _managerStub;
        private readonly UnitOfWorkServiceRegistrarFixture _fixture;

        public UnitOfWorkEventPublishingManagerTests(UnitOfWorkServiceRegistrarFixture fixture)
        {
            _fixture = fixture;

            _storeStub = new Mock<IUnitOfWorkEventPublishingStore>();
            _managerStub = new Mock<UnitOfWorkEventPublishingManager>(_storeStub.Object)
            {
                CallBase = true
            };
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventPublishingManager))]
        [Trait("Method", nameof(UnitOfWorkEventPublishingManager.CreateAndInsertAsync))]
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

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventPublishingManager))]
        [Trait("Method", nameof(UnitOfWorkEventPublishingManager.CreateAndInsertAsync))]
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

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventPublishingManager))]
        [Trait("Method", nameof(UnitOfWorkEventPublishingManager.CreateAndInsertAsync))]
        [Fact]
        public void Given_Publisher_When_CreateAndInsertPublishing_Then_ThrowArgumentError()
        {
            var publisher = _fixture.ServiceProvider.GetService<ILocalEventBus>();
            var action = async () => await _managerStub.Object.CreateAndInsertAsync(publisher, null);

            action.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventPublishingManager))]
        [Trait("Method", nameof(UnitOfWorkEventPublishingManager.CreateAndInsertAsync))]
        [Fact]
        public void Given_Null_When_CreateAndInsertPublishing_Then_ThrowArgumentError()
        {
            var action = async () => await _managerStub.Object.CreateAndInsertAsync(null, null);

            action.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventPublishingManager))]
        [Trait("Method", nameof(UnitOfWorkEventPublishingManager.InsertAsync))]
        [Fact]
        public async Task Given_Publishing_When_InsertPublishing_Then_Successful()
        {
            var publisher = _fixture.ServiceProvider.GetService<ILocalEventBus>();
            var eventRecord = new UnitOfWorkEventRecord(new(), new());
            var publishing = new UnitOfWorkEventPublishing(It.IsAny<IEventBus>(), It.IsAny<UnitOfWorkEventRecord>(), 1);

            _storeStub.Setup(m => m.PushAsync(It.IsAny<UnitOfWorkEventPublishing>(), It.IsAny<CancellationToken>()));

            await _managerStub.Object.InsertAsync(publishing, It.IsAny<CancellationToken>());

        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventPublishingManager))]
        [Trait("Method", nameof(UnitOfWorkEventPublishingManager.InsertAsync))]
        [Fact]
        public void Given_Null_When_InsertPublishing_Then_Successful()
        {
            var action = async () => await _managerStub.Object.InsertAsync(null, It.IsAny<CancellationToken>());

            action.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventPublishingManager))]
        [Trait("Method", nameof(UnitOfWorkEventPublishingManager.PublishAllAsync))]
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

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventPublishingManager))]
        [Trait("Method", nameof(UnitOfWorkEventPublishingManager.PublishAllAsync))]
        [Fact]
        public async Task Given_ThereAreNotRegisteredItems_When_PublishAllPublishing_Then_Successful()
        {
            var unorderedItems = new List<UnitOfWorkEventPublishing>();

            _storeStub.Setup(s => s.Get(null)).Returns(unorderedItems);

            await _managerStub.Object.PublishAllAsync();
        }
    }
}
