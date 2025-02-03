using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Vesta.Banks.Dtos;
using Vesta.Banks.Fixtures;
using Vesta.Banks.Traceability;
using Vesta.TestBase.Fixtures;
using Xunit;

namespace Vesta.Banks.Application
{
    public class BankAppServiceTests : IClassFixture<ApplicationServiceRegistrarFixture>
    {
        private readonly Mock<BankAppService> _service;
        private readonly Mock<IDistributedCache> _distributedCacheStub;
        private readonly Mock<IBankAccountRepository> _bankAccountRepositoryStub;
        private readonly Mock<IBankTransferRepository> _bankTransferRepositoryStub;
        private readonly Mock<IErrorRepository> _errorRepositoryStub;
        private readonly Mock<IBankAccountManager> _bankAccountManagerStub;
        private readonly Mock<IBankTransferService> _bankTransferServiceStub;
        private readonly Mock<IBankAccountPublisher> _bankAccountPublisherStub;

        public BankAppServiceTests(ApplicationServiceRegistrarFixture fixture)
        {

            _distributedCacheStub = new Mock<IDistributedCache>();
            _bankAccountRepositoryStub = new Mock<IBankAccountRepository>();
            _bankTransferRepositoryStub = new Mock<IBankTransferRepository>();
            _errorRepositoryStub = new Mock<IErrorRepository>();
            _bankAccountManagerStub = new Mock<IBankAccountManager>();
            _bankTransferServiceStub = new Mock<IBankTransferService>();
            _bankAccountPublisherStub = new Mock<IBankAccountPublisher>();

            _service = new Mock<BankAppService>(
                _distributedCacheStub.Object,
                _bankAccountRepositoryStub.Object,
                _bankTransferRepositoryStub.Object,
                _errorRepositoryStub.Object,
                _bankAccountManagerStub.Object,
                _bankTransferServiceStub.Object,
                _bankAccountPublisherStub.Object);

            _service.Object.ServiceProvider = fixture.ServiceProvider;

        }

        [Fact]
        public async Task When_GetAllBankTransferList_Then_ReturnBankTransferList()
        {
            const int EQUAL_BANK_TRASNFER_LIST_COUNT = 1;
            var bankTransferList = new List<BankTransfer>()
            {
                new BankTransfer("***123***", "***321***", 0)
            };

            _bankTransferRepositoryStub.Setup(r => r.GelAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(bankTransferList);

           var dtos = await _service.Object.GetAllBankTransferListAsync(It.IsAny<CancellationToken>());

            dtos.Should().BeOfType<List<BankTransferOutput>>()
                .And.Subject.Should().HaveCount(EQUAL_BANK_TRASNFER_LIST_COUNT);
        }

        [Fact]
        public void When_GetAllBankTransferList_Then_ThrowAnyError()
        {

            _bankTransferRepositoryStub.Setup(r => r.GelAllAsync(It.IsAny<CancellationToken>())).ThrowsAsync(It.IsAny<Exception>());

            var action = async () => await _service.Object.GetAllBankTransferListAsync(It.IsAny<CancellationToken>());

            action.Should().ThrowExactlyAsync<Exception>();
        }
    }
}