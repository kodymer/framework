using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyName.EventBus.Abstractions;
using CompanyName.TestBase;
using Xunit;

namespace CompanyName.Uow
{
    public class InMemoryEventStoreTests
    {
        private readonly Mock<InMemoryEventStore> _storeStub;

        public InMemoryEventStoreTests()
        {
            _storeStub = new Mock<InMemoryEventStore>() { 
                CallBase = true 
            };
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(InMemoryEventStore))]
        [Trait("Method", nameof(InMemoryEventStore.PushAsync))]
        [Fact]
        public async Task Given_Publishing_When_AddPublishingToQueue_Then_ItIsAdded()
        {
            var publishing = new UnitOfWorkEventPublishing(It.IsAny<IEventBus>(), It.IsAny<UnitOfWorkEventRecord>(), 1);

            await _storeStub.Object.PushAsync(publishing);
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(InMemoryEventStore))]
        [Trait("Method", nameof(InMemoryEventStore.Get))]
        [Fact]
        public  void Given_Predicate_When_GetPublishing_Then_ReturnFilteredPublishing()
        {
            var publishing = _storeStub.Object.Get(((UnitOfWorkEventPublishing publishing, long priority) item) => true);

            publishing.Should().NotBeNull();
        }

        [Trait("Category", CompanyNameUnitTestCategories.Data)]
        [Trait("Class", nameof(InMemoryEventStore))]
        [Trait("Method", nameof(InMemoryEventStore.Get))]
        [Fact]
        public void Given_Null_When_GetPublishing_Then_ReturnFilteredPublishing()
        {
            var publishing = _storeStub.Object.Get();

            publishing.Should().NotBeNull();
        }

    }
}
