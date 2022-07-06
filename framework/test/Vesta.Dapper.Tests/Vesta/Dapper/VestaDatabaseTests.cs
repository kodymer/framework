using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.Ddd.Domain.Entities;
using Vesta.TestBase;
using Xunit;

namespace Vesta.Dapper
{
    public class VestaDatabaseTests
    {

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(VestaDatabase<DefaultVestaDatabase>))]
        [Trait("Method", nameof(VestaDatabase<DefaultVestaDatabase>.Init))]
        [Fact]
        public void Given_Connection_When_TryInitializateDatabase_Then_Successful()
        {
            var connectionStub = new Mock<DbConnection>();

            DefaultVestaDatabase.Init(connectionStub.Object);

        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(VestaDatabase<DefaultVestaDatabase>))]
        [Trait("Method", nameof(VestaDatabase<DefaultVestaDatabase>.Table))]
        [Fact]
        public void Given_Connection_When_GettingTable_Then_ReturnTable()
        {
            var connectionStub = new Mock<DbConnection>();
            var database = DefaultVestaDatabase.Init(connectionStub.Object);

            var table = database.Table<VestaEntity, int>();

            table.Should().NotBeNull();

        }

        private class DefaultVestaDatabase : VestaDatabase<DefaultVestaDatabase>
        {
            protected override void OnModelCreating(IModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<VestaEntity>(b =>
                {
                    b.ToTable(VestaEntity.TableName);
                });
            }
        }

        private class VestaEntity : IEntity<int>
        {
            public const string TableName = "***table-name***";

            public int Id { get; }
        }
    }
}
