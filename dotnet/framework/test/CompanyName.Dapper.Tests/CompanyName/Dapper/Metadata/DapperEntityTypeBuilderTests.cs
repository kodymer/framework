//using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyName.Ddd.Domain.Entities;
using CompanyName.TestBase;
using Xunit;

namespace CompanyName.Dapper.Metadata
{
    public class DapperEntityTypeBuilderTests
    {

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(DapperEntityTypeBuilder<CompanyNameEntity>))]
        [Trait("Method", nameof(DapperEntityTypeBuilder<CompanyNameEntity>.ToTable))]
        [Fact]
        public void Given_EntityType_When_AddTableName_Then_Successful()
        {
            var builder = new DapperEntityTypeBuilder<CompanyNameEntity>();
            var tables = builder.ToTable(It.IsAny<string>()).As<IEntityTypeBuilder>().Tables;

            Assert.True(tables.ContainsKey(typeof(CompanyNameEntity)));
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(DapperEntityTypeBuilder<CompanyNameEntity>))]
        [Trait("Method", nameof(DapperEntityTypeBuilder<CompanyNameEntity>.ToTable))]
        [Fact]
        public void Given_EntityType_When_AddExistingTableName_Then_Successful()
        {
            const string EQUAL_VALUE = "***new-table-name***";

            var builder = new DapperEntityTypeBuilder<CompanyNameEntity>();
            builder.ToTable(It.IsAny<string>());
            var tables = builder.ToTable(EQUAL_VALUE).As<IEntityTypeBuilder>().Tables;

            Assert.True(tables.TryGetValue(typeof(CompanyNameEntity), out var value));
            Assert.Equal(EQUAL_VALUE, value);
        }

        private class CompanyNameEntity 
        {

        }
    }
}
