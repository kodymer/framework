using Ardalis.Specification;
using CompanyName.Data.Fixtures;
using CompanyName.Ddd.Domain.Entities;
using CompanyName.EntityFrameworkCore.Repositories;
using CompanyName.TestBase;
using CompanyName.TestBase.Orderers;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
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

            _repositoryStub = new(_fixture.DbContext)
            {
                CallBase = true
            };

            _fixture.DbContext.ChangeTracker.Clear();
        }


        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>.AddAsync))]
        [Theory, Order(1)]
        [InlineData(1)]
        public void Given_Entity_When_Insert_Then_Successful(int id)
        {
            var entity = new CompanyNameAggregateRoot(id);

            var action = async () => await _repositoryStub.Object.AddAsync(entity, false);

            action.Should().NotThrowAsync();
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
        public async Task Given_EntityWithoutAutoSave_When_Update_Then_NoRecordsAreAffected()
        {
            const int ENTITY_ID = 0;
            var entity = new CompanyNameAggregateRoot(ENTITY_ID);

            int result = await _repositoryStub.Object.UpdateAsync(entity, false);

            result.Should().Be(0);
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>.UpdateAsync))]
        [Fact, Order(4)]
        public async Task Given_EntityWithAutoSave_When_Update_Then_OneRecordIsAffected()
        {
            const int ENTITY_ID = 1;
            const int ENTITY_AFFECTED_COUNT = 1;
            var entity = new CompanyNameAggregateRoot(ENTITY_ID);
            var token = CancellationToken.None;

            _fixture.DbContext.Set<CompanyNameAggregateRoot>().Add(entity);
            await _fixture.DbContext.SaveChangesAsync();

            int result = await _repositoryStub.Object.UpdateAsync(entity, true, token);

            result.Should().Be(ENTITY_AFFECTED_COUNT);
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>.UpdateAsync))]
        [Fact, Order(5)]
        public void Given_Null_When_Update_Then_ThrowArgumentError()
        {
            var action = async () => await _repositoryStub.Object.UpdateAsync(null, false);

            action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>.DeleteAsync))]
        [Fact, Order(6)]
        public void Given_Null_When_Delete_Then_ThrowArgumentError()
        {
            var action = async () => await _repositoryStub.Object.DeleteAsync(null, false);

            action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>))]
        [Trait("Method", nameof(EfCoreRepository<InMemoryCompanyNameDbContext, CompanyNameAggregateRoot>.DeleteAsync))]
        [Fact, Order(7)]
        public void Given_NotExistingId_When_Delete_Then_ThrowEntityNotFoundError()
        {
            const int NOT_EXISTING_ID = 0;

            var action = async () => await _repositoryStub.Object.DeleteAsync(NOT_EXISTING_ID);

            action.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }
    }
}