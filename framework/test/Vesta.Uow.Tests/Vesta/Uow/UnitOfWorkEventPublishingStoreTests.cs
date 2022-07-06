using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.EventBus.Abstracts;
using Vesta.TestBase;
using Xunit;

namespace Vesta.Uow
{
    public class UnitOfWorkEventPublishingStoreTests
    {
        private readonly Mock<UnitOfWorkEventPublishingStore> _storeStub;

        public UnitOfWorkEventPublishingStoreTests()
        {
            _storeStub = new Mock<UnitOfWorkEventPublishingStore>() { 
                CallBase = true 
            };
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventPublishingStore))]
        [Trait("Method", nameof(UnitOfWorkEventPublishingStore.PushAsync))]
        [Fact]
        public async Task Given_Publishing_When_AddPublishingToQueue_Then_ItIsAdded()
        {
            var publishing = new UnitOfWorkEventPublishing(It.IsAny<IEventBus>(), It.IsAny<UnitOfWorkEventRecord>(), 1);

            await _storeStub.Object.PushAsync(publishing);
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventPublishingStore))]
        [Trait("Method", nameof(UnitOfWorkEventPublishingStore.Get))]
        [Fact]
        public  void Given_Predicate_When_GetPublishing_Then_ReturnFilteredPublishing()
        {
            var publishing = _storeStub.Object.Get(((UnitOfWorkEventPublishing publishing, long priority) item) => true);

            publishing.Should().NotBeNull();
        }

        [Trait("Category", VestaUnitTestCategories.Data)]
        [Trait("Class", nameof(UnitOfWorkEventPublishingStore))]
        [Trait("Method", nameof(UnitOfWorkEventPublishingStore.Get))]
        [Fact]
        public void Given_Null_When_GetPublishing_Then_ReturnFilteredPublishing()
        {
            var publishing = _storeStub.Object.Get();

            publishing.Should().NotBeNull();
        }

    }
}
