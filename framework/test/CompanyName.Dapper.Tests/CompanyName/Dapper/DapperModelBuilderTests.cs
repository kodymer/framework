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
    public class DapperModelBuilderTests
    {

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(DapperModelBuilder))]
        [Trait("Method", nameof(DapperModelBuilder.Entity))]
        [Fact]
        public void Given_EntityType_When_AddEntityTypeBuilder_Then_Successful()
        {
            var builder = new DapperModelBuilder();
            var entityTypeBuilders = builder.Entity<CompanyNameEntity>(e => { e.ToTable(It.IsAny<string>()); })
                .As<IModelBuilder>().EntityTypeBuilders;

            Assert.True(entityTypeBuilders.ContainsKey(typeof(CompanyNameEntity)));
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(DapperModelBuilder))]
        [Trait("Method", nameof(DapperModelBuilder.Entity))]
        [Fact]
        public void Given_EntityType_When_AddExistingEntityTypeBuilder_Then_Successful()
        {

            Action<DapperEntityTypeBuilder<CompanyNameEntity>> EQUAL_ACTION_BUILDER = e => { e.ToTable("***table-name***"); };
         
            var builder = new DapperModelBuilder();
            builder.Entity<CompanyNameEntity>(e => { e.ToTable(It.IsAny<string>()); });
            var entityTypeBuilders = builder.Entity(EQUAL_ACTION_BUILDER).As<IModelBuilder>().EntityTypeBuilders;

            Assert.True(entityTypeBuilders.TryGetValue(typeof(CompanyNameEntity), out var value));
            Assert.Equal(EQUAL_ACTION_BUILDER, value);
        }

        private class CompanyNameEntity 
        {

        }
    }
}
