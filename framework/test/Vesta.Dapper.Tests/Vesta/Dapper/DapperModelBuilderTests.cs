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
    public class DapperModelBuilderTests
    {

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(DapperModelBuilder))]
        [Trait("Method", nameof(DapperModelBuilder.Entity))]
        [Fact]
        public void Given_EntityType_When_AddEntityTypeBuilder_Then_Successful()
        {
            var builder = new DapperModelBuilder();
            var entityTypeBuilders = builder.Entity<VestaEntity>(e => { e.ToTable(It.IsAny<string>()); })
                .As<IModelBuilder>().EntityTypeBuilders;

            Assert.True(entityTypeBuilders.ContainsKey(typeof(VestaEntity)));
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(DapperModelBuilder))]
        [Trait("Method", nameof(DapperModelBuilder.Entity))]
        [Fact]
        public void Given_EntityType_When_AddExistingEntityTypeBuilder_Then_Successful()
        {

            Action<DapperEntityTypeBuilder<VestaEntity>> EQUAL_ACTION_BUILDER = e => { e.ToTable("***table-name***"); };
         
            var builder = new DapperModelBuilder();
            builder.Entity<VestaEntity>(e => { e.ToTable(It.IsAny<string>()); });
            var entityTypeBuilders = builder.Entity(EQUAL_ACTION_BUILDER).As<IModelBuilder>().EntityTypeBuilders;

            Assert.True(entityTypeBuilders.TryGetValue(typeof(VestaEntity), out var value));
            Assert.Equal(EQUAL_ACTION_BUILDER, value);
        }

        private class VestaEntity 
        {

        }
    }
}
