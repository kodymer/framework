//using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.Ddd.Domain.Entities;
using Vesta.TestBase;
using Xunit;

namespace Vesta.Dapper.Metadata
{
    public class DapperEntityTypeBuilderTests
    {

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(DapperEntityTypeBuilder<VestaEntity>))]
        [Trait("Method", nameof(DapperEntityTypeBuilder<VestaEntity>.ToTable))]
        [Fact]
        public void Given_EntityType_When_AddTableName_Then_Successful()
        {
            var builder = new DapperEntityTypeBuilder<VestaEntity>();
            var tables = builder.ToTable(It.IsAny<string>()).As<IEntityTypeBuilder>().Tables;

            Assert.True(tables.ContainsKey(typeof(VestaEntity)));
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(DapperEntityTypeBuilder<VestaEntity>))]
        [Trait("Method", nameof(DapperEntityTypeBuilder<VestaEntity>.ToTable))]
        [Fact]
        public void Given_EntityType_When_AddExistingTableName_Then_Successful()
        {
            const string EQUAL_VALUE = "***new-table-name***";

            var builder = new DapperEntityTypeBuilder<VestaEntity>();
            builder.ToTable(It.IsAny<string>());
            var tables = builder.ToTable(EQUAL_VALUE).As<IEntityTypeBuilder>().Tables;

            Assert.True(tables.TryGetValue(typeof(VestaEntity), out var value));
            Assert.Equal(EQUAL_VALUE, value);
        }

        private class VestaEntity 
        {

        }
    }
}
