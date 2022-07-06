using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Vesta.Data.Fixtures;
using Vesta.EntityFrameworkCore;
using Vesta.TestBase;
using Vesta.TestBase.Fixtures;
using Vesta.TestBase.Orderers;
using Vesta.Uow.EntityFrameworkCore;
using Xunit;

namespace Vesta.Uow.Tests
{
    [TestCaseOrderer("Vesta.TestBase.Orderers.PriorityOrderer", "Vesta.TestBase")]
    [Collection(nameof(DataContextCollection))]
    public class UnitOfWorkTests : IClassFixture<ServiceRegistrarFixture>, IClassFixture<InMemoryDbContextFixture>
    {
        private readonly Mock<UnitOfWork> _unitOfWorkStub;
        private readonly ServiceRegistrarFixture _serviceRegistrarFixture;
        private readonly InMemoryDbContextFixture _inMemoryDbContextFixture;

        public UnitOfWorkTests(
            ServiceRegistrarFixture serviceRegistrarFixture,
            InMemoryDbContextFixture inMemoryDbContextFixture
            )
        {
            _serviceRegistrarFixture = serviceRegistrarFixture;
            _inMemoryDbContextFixture = inMemoryDbContextFixture;

            _unitOfWorkStub = new Mock<UnitOfWork>(_serviceRegistrarFixture.ServiceProvider);
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
    }
}