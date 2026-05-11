using CompanyName.Data.Fixtures;
using CompanyName.Ddd.Domain.Entities;
using CompanyName.EntityFrameworkCore.Repositories;
using CompanyName.TestBase.Orderers;
using CompanyName.Data.Fixtures;
using CompanyName.Ddd.Domain.Entities;
using CompanyName.EntityFrameworkCore.Repositories;
using CompanyName.TestBase;
using CompanyName.TestBase.Orderers;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CompanyName.Domain.EntityFrameworkCore
{
    [TestCaseOrderer("CompanyName.TestBase.Orderers.PriorityOrderer", "CompanyName.TestBase")]
    public class RepositoryTests : IClassFixture<InMemoryDbContextFixture>
    {
        private readonly InMemoryDbContextFixture _fixture;
        private readonly Mock<EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>> _repositoryStub;

        public RepositoryTests(InMemoryDbContextFixture fixture)
        {
            _fixture = fixture;

            _repositoryStub = new(_fixture.DbContext);
            _repositoryStub.CallBase = true;
        }


        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>.AddAsync))]
        [Theory, Order(1)]
        [InlineData(1)]
        public void Given_Entity_When_Insert_Then_Successful(int id)
        {
            var entity = new CompanyNameAggregateRoot(id);

            Action action = async () => await _repositoryStub.Object.AddAsync(entity, false);

            action.Should().NotThrow();
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>.AddAsync))]
        [Fact, Order(2)]
        public void Given_Null_When_Insert_Then_ThrowArgumentError()
        {
            var action = async () => await _repositoryStub.Object.AddAsync(null, false);

            action.Should().ThrowAsync<ArgumentNullException>();
        }


        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>.UpdateAsync))]
        [Fact, Order(3)]
        public async Task Given_Entity_When_Update_Then_Successful()
        {
            var entity = await _repositoryStub.Object.GetByIdAsync(1);

            Action action = async () => await _repositoryStub.Object.UpdateAsync(entity, false);

            action.Should().NotThrow();
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>.UpdateAsync))]
        [Fact, Order(4)]
        public void Given_Null_When_Update_Then_ThrowArgumentError()
        {
            var action = async () => await _repositoryStub.Object.UpdateAsync(null, false);

            action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>.DeleteAsync))]
        [Fact, Order(8)]
        public void Given_Null_When_Delete_Then_ThrowArgumentError()
        {
            var action = async () => await _repositoryStub.Object.DeleteAsync(null, false);

            action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>.DeleteAsync))]
        [Fact, Order(9)]
        public void Given_NotExistingId_When_Delete_Then_ThrowEntityNotFoundError()
        {
            const int NOT_EXISTING_ID = 0;

            var action = async () => await _repositoryStub.Object.DeleteAsync(NOT_EXISTING_ID);

            action.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }
    }
}