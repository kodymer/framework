using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyName.TestBase;
using Xunit;
using CompanyName.Ddd.Domain.Entities;

namespace CompanyName.Dapper
{
    public class CompanyNameDatabaseTests
    {

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(CompanyNameDatabase<DefaultCompanyNameDatabase>))]
        [Trait("Method", nameof(CompanyNameDatabase<DefaultCompanyNameDatabase>.Init))]
        [Fact]
        public void Given_Connection_When_TryInitializateDatabase_Then_Successful()
        {
            var connectionStub = new Mock<DbConnection>();

            DefaultCompanyNameDatabase.Init(connectionStub.Object);

        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(CompanyNameDatabase<DefaultCompanyNameDatabase>))]
        [Trait("Method", nameof(CompanyNameDatabase<DefaultCompanyNameDatabase>.Table))]
        [Fact]
        public void Given_Connection_When_GettingTable_Then_ReturnTable()
        {
            var connectionStub = new Mock<DbConnection>();
            var database = DefaultCompanyNameDatabase.Init(connectionStub.Object);

            var table = database.Table<CompanyNameEntity, int>();

            table.Should().NotBeNull();

        }

        private class DefaultCompanyNameDatabase : CompanyNameDatabase<DefaultCompanyNameDatabase>
        {
            protected override void OnModelCreating(IModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<CompanyNameEntity>(b =>
                {
                    b.ToTable(CompanyNameEntity.TableName);
                });
            }
        }

        private class CompanyNameEntity : IEntity<int>
        {
            public const string TableName = "***table-name***";

            public int Id { get; }
        }
    }
}
