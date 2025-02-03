using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using CompanyName.Data.Fixtures;
using CompanyName.Ddd.Domain.Entities;
using CompanyName.Domain.EntityFrameworkCore.Repositories;
using CompanyName.TestBase;
using CompanyName.TestBase.Orderers;
using Xunit;

namespace CompanyName.EntityFrameworkCore
{
    [TestCaseOrderer("CompanyName.TestBase.Orderers.PriorityOrderer", "CompanyName.TestBase")]
    public class EfCoreRepositoryTests : IClassFixture<InMemoryDbContextFixture>
    {
        private readonly InMemoryDbContextFixture _fixture;
        private readonly Mock<EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>> _repositoryStub;

        public EfCoreRepositoryTests(InMemoryDbContextFixture fixture)
        {
            _fixture = fixture;

            _repositoryStub = new(_fixture.DbContextProvider);
            _repositoryStub.CallBase = true;
            _repositoryStub.Setup(r => r.GetDbContextAsync()).Returns(Task.FromResult(_fixture.DbContext));
        }


        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>.InsertAsync))]
        [Theory, Order(1)]
        [InlineData(1, true)]
        [InlineData(2, false)]
        public async Task Given_Entity_When_Insert_Then_Successful(int id, bool autoSave)
        {
            var entity = new CompanyNameEntity(id);

            await _repositoryStub.Object.InsertAsync(entity, autoSave);

        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>.InsertAsync))]
        [Fact, Order(2)]
        public void Given_Null_When_Insert_Then_ThrowArgumentError()
        {
            var action = async () => await _repositoryStub.Object.InsertAsync(null);

            action.Should().ThrowAsync<ArgumentNullException>();
        }


        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>.UpdateAsync))]
        [Theory, Order(3)]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Given_Entity_When_Update_Then_Successful(bool autoSave)
        {
            var entity = await _repositoryStub.Object.GetAsync(1);

            await _repositoryStub.Object.UpdateAsync(entity, autoSave);

        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>.UpdateAsync))]
        [Fact, Order(4)]
        public void Given_Null_When_Update_Then_ThrowArgumentError()
        {
            var action = async () => await _repositoryStub.Object.UpdateAsync(null);

            action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>.GetAsync))]
        [Fact, Order(5)]
        public async Task Given_Id_When_Get_Then_ReturnEntity()
        {
            const int EXISTING_ID = 1;

            var expectedEntity = await _repositoryStub.Object.GetAsync(EXISTING_ID);

            expectedEntity.Should().NotBeNull();
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>.GetAsync))]
        [Fact, Order(6)]
        public void Given_NotExistingId_When_Get_Then_ThrowEntityNotFoundError()
        {
            const int NOT_EXISTING_ID = 0;

            var action = async () => await _repositoryStub.Object.GetAsync(NOT_EXISTING_ID);

            action.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>.DeleteAsync))]
        [Theory, Order(7)]
        [InlineData(1, true)]
        [InlineData(2, false)]
        public async Task Given_Entity_When_Delete_Then_Successful(int id, bool autoSave)
        {
            var entity = await _repositoryStub.Object.GetAsync(id);

            await _repositoryStub.Object.DeleteAsync(entity, autoSave);
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>.DeleteAsync))]
        [Fact, Order(8)]
        public void Given_Null_When_Delete_Then_ThrowArgumentError()
        {
            var action = async () => await _repositoryStub.Object.DeleteAsync(null);

            action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameEntity, int>.DeleteAsync))]
        [Fact, Order(9)]
        public void Given_NotExistingId_When_Delete_Then_ThrowEntityNotFoundError()
        {
            const int NOT_EXISTING_ID = 0;

            var action = async () => await _repositoryStub.Object.DeleteAsync(NOT_EXISTING_ID);

            action.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }
    }
}