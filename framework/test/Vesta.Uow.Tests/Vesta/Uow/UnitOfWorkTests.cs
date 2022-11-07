using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vesta.Data.Fixtures;
using Vesta.EntityFrameworkCore;
using Vesta.EventBus.Abstracts;
using Vesta.TestBase;
using Vesta.TestBase.Fixtures;
using Vesta.TestBase.Orderers;
using Vesta.Uow.EntityFrameworkCore;
using Vesta.Uow.Fixtures;
using Xunit;

namespace Vesta.Uow.Tests
{
    [TestCaseOrderer("Vesta.TestBase.Orderers.PriorityOrderer", "Vesta.TestBase")]
    [Collection(nameof(UnitOfWorkCollectionFixture))]
    public class UnitOfWorkTests : IClassFixture<UnitOfWorkServiceRegistrarFixture>, IClassFixture<InMemoryDbContextFixture>
    {
        private readonly Mock<UnitOfWork> _unitOfWorkStub;
        private readonly Mock<IUnitOfWorkEventPublishingManager> _unitOfWorkeventPublishingManagerStub;

        private readonly ServiceRegistrarFixture _serviceRegistrarFixture;
        private readonly InMemoryDbContextFixture _inMemoryDbContextFixture;

        public UnitOfWorkTests(
            UnitOfWorkServiceRegistrarFixture serviceRegistrarFixture,
            InMemoryDbContextFixture inMemoryDbContextFixture
            )
        {
            _serviceRegistrarFixture = serviceRegistrarFixture;
            _inMemoryDbContextFixture = inMemoryDbContextFixture;

            _unitOfWorkeventPublishingManagerStub = new Mock<IUnitOfWorkEventPublishingManager>();

            var options = Options.Create(new UnitOfWorkDefaultOptions());

            _unitOfWorkStub = new Mock<UnitOfWork>(
                _serviceRegistrarFixture.ServiceProvider,
                _unitOfWorkeventPublishingManagerStub.Object,
                options)
            {
                CallBase = true
            };
        }

        private void AddDatabaseApi(out string key)
        {
            key = "***key***";
            var databaseApi = new EfCoreDatabaseApi(_inMemoryDbContextFixture.DbContext);

            _unitOfWorkStub.Object.AddDatabaseApi(key, databaseApi);
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWork))]
        [Trait("Method", nameof(UnitOfWork.AddDatabaseApi))]
        [Fact, Order(1)]
        public void Given_KeyAndDatabaseApi_When_RegistrarDatabaseApi_Then_DatabaseApiAdded()
        {
            AddDatabaseApi(out string key);
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWork))]
        [Trait("Method", nameof(UnitOfWork.FindDatabaseApi))]
        [Fact, Order(2)]
        public void Given_KeyAndDatabaseApi_When_FindDatabaseApiRegistered_Then_ReturnDatabaseApiFinded()
        {
            AddDatabaseApi(out string key);

            var databaseApiExpected= _unitOfWorkStub.Object.FindDatabaseApi(key);

            databaseApiExpected.Should().NotBeNull();
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWork))]
        [Trait("Method", nameof(UnitOfWork.AddDatabaseApi))]
        [Fact, Order(3)]
        public void Given_KeyAndDatabaseApi_When_RegistrarDatabaseApiAlreadyRegistered_Then_ThrowInvalidOperationError()
        {
            var databaseApiDuplicated = new EfCoreDatabaseApi(_inMemoryDbContextFixture.DbContext);

            AddDatabaseApi(out string key);

            var action = () => _unitOfWorkStub.Object.AddDatabaseApi(key, databaseApiDuplicated);

            action.Should().ThrowExactly<InvalidOperationException>();
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWork))]
        [Trait("Method", nameof(UnitOfWork.AddDatabaseApi))]
        [Fact, Order(4)]
        public void Given_DatabaseApi_When_RegistrarDatabaseApi_Then_ThrowArgumentError()
        {
            var databaseApi = new EfCoreDatabaseApi(_inMemoryDbContextFixture.DbContext);

            var action = () => _unitOfWorkStub.Object.AddDatabaseApi(null, databaseApi);

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWork))]
        [Trait("Method", nameof(UnitOfWork.AddDatabaseApi))]
        [Fact, Order(5)]
        public void Given_Key_When_RegistrarDatabaseApi_Then_ThrowArgumentError()
        {
            var key = "***key***";
            var databaseApi = new EfCoreDatabaseApi(_inMemoryDbContextFixture.DbContext);

            var action = () => _unitOfWorkStub.Object.AddDatabaseApi(key, null);

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWork))]
        [Trait("Method", nameof(UnitOfWork.SaveChangesAsync))]
        [Fact, Order(6)]
        public async Task When_SaveChangesForAllContext_Then_Saved()
        {
            AddDatabaseApi(out string key);

            await _unitOfWorkStub.Object.SaveChangesAsync();
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWork))]
        [Trait("Method", nameof(UnitOfWork.CompleteAsync))]
        [Fact, Order(6)]
        public async Task When_Complete_Then_Successful()
        {
            AddDatabaseApi(out string key);

            _unitOfWorkeventPublishingManagerStub.Setup(p => p.PublishAllAsync(It.IsAny<CancellationToken>()));

            await _unitOfWorkStub.Object.CompleteAsync();

            _unitOfWorkStub.Object.IsCompleted.Should().BeTrue();
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWork))]
        [Trait("Method", nameof(UnitOfWork.CompleteAsync))]
        [Fact, Order(6)]
        public async Task When_ItIsCompleted_Then_ThrowCompletedEvent()
        {
            AddDatabaseApi(out string key);

            _unitOfWorkStub.Object.Completed += UnitOfWorkCompletedEventHandler; ;

            _unitOfWorkeventPublishingManagerStub.Setup(p => p.PublishAllAsync(It.IsAny<CancellationToken>()));

            await _unitOfWorkStub.Object.CompleteAsync();

            void UnitOfWorkCompletedEventHandler(object sender, EventArgs args)
            {
                sender.Should().NotBeNull();
                args.Should().BeSameAs(EventArgs.Empty);
            }
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWork))]
        [Trait("Method", nameof(UnitOfWork.CompleteAsync))]
        [Fact, Order(6)]
        public async Task When_ItIsCompleting_Then_ThrowCompletingEvent()
        {
            AddDatabaseApi(out string key);

            _unitOfWorkStub.Object.Completing += UnitOfWorkCompletingEventHandler; ;

            _unitOfWorkeventPublishingManagerStub.Setup(p => p.PublishAllAsync(It.IsAny<CancellationToken>()));

            await _unitOfWorkStub.Object.CompleteAsync();

            void UnitOfWorkCompletingEventHandler(object sender, EventArgs args)
            {
                sender.Should().NotBeNull();
                args.Should().BeSameAs(EventArgs.Empty);
            }
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWork))]
        [Trait("Method", nameof(UnitOfWork.CompleteAsync))]
        [Fact, Order(6)]
        public void When_CompletingAndTryCompleteAgain_Then_ThrowInvalidOperationError()
        {

            _unitOfWorkStub.SetupGet(u => u.IsCompleting).Returns(true);

            var action = async () => await _unitOfWorkStub.Object.CompleteAsync();

            action.Should().ThrowExactlyAsync<InvalidOperationException>();
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWork))]
        [Trait("Method", nameof(UnitOfWork.CompleteAsync))]
        [Fact, Order(6)]
        public void When_CompletedAndTryCompleteAgain_Then_ThrowInvalidOperationError()
        {

            _unitOfWorkStub.SetupGet(u => u.IsCompleted).Returns(true);

            var action = async () => await _unitOfWorkStub.Object.CompleteAsync();

            action.Should().ThrowExactlyAsync<InvalidOperationException>();
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWork))]
        [Trait("Method", nameof(UnitOfWork.AddEventRecordAsync))]
        [Fact, Order(6)]
        public async Task Given_EventRecord_When_AddEventRecordToQueue_Then_Successful()
        {
            var unitOrWorkEventRecord = new UnitOfWorkEventRecord(new(), new());

            _unitOfWorkeventPublishingManagerStub
                .Setup(p => p.CreateAndInsertAsync(It.IsAny<IEventBus>(), It.IsAny<UnitOfWorkEventRecord>(), It.IsAny<long>(), It.IsAny<CancellationToken>()));

            await _unitOfWorkStub.Object.AddEventRecordAsync<IDistributedEventBus>(unitOrWorkEventRecord, 1, It.IsAny<CancellationToken>());
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWork))]
        [Trait("Method", nameof(UnitOfWork.AddEventRecordAsync))]
        [Fact, Order(6)]
        public void Given_Null_When_AddEventRecord_Then_ThrowArgumentError()
        {

            var action = async () => await _unitOfWorkStub.Object.AddEventRecordAsync<IDistributedEventBus>(null, 1, It.IsAny<CancellationToken>());

            action.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
    }
}