using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Samples.Banks.Accounts;
using Samples.Banks.Fixtures;
using Samples.Banks.MoneyTransfer;
using Samples.Banks.MoneyTransfers;
using Samples.Banks.Traceability;
using Samples.Banks.Transfers;
using Xunit;

namespace Samples.Banks.Application
{
    public class MoneyTransferAppServiceTests : IClassFixture<ApplicationServiceRegistrarFixture>
    {
        private readonly Mock<IDistributedCache> _distributedCacheStub;
        private readonly Mock<MoneyTransferAppService> _service;
        private readonly Mock<IBankAccountRepository> _bankAccountRepositoryStub;
        private readonly Mock<IBankTransferRepository> _bankTransferRepositoryStub;
        private readonly Mock<IErrorRepository> _errorRepositoryStub;
        private readonly Mock<IBankTransferService> _bankTransferServiceStub;


        public MoneyTransferAppServiceTests(ApplicationServiceRegistrarFixture fixture)
        {

            _distributedCacheStub = new Mock<IDistributedCache>();
            _bankAccountRepositoryStub = new Mock<IBankAccountRepository>();
            _bankTransferRepositoryStub = new Mock<IBankTransferRepository>();
            _errorRepositoryStub = new Mock<IErrorRepository>();
            _bankTransferServiceStub = new Mock<IBankTransferService>();

            _service = new Mock<MoneyTransferAppService>(
                _distributedCacheStub.Object,
                _bankAccountRepositoryStub.Object,
                _bankTransferRepositoryStub.Object,
                _bankTransferServiceStub.Object);

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

            _bankTransferRepositoryStub.Setup(repository => repository.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(bankTransferList);

            var result = await _service.Object.GetTransferHistoryAsync(It.IsAny<CancellationToken>());


            result.Should().BeSuccess();
            result.Value.Should().BeOfType<List<BankTransferDto>>()
                .And.Subject.Should().HaveCount(EQUAL_BANK_TRASNFER_LIST_COUNT);
        }

        [Fact]
        public void When_GetAllBankTransferList_Then_ThrowAnyError()
        {

            _bankTransferRepositoryStub.Setup(repository => repository.ListAsync(It.IsAny<CancellationToken>())).ThrowsAsync(It.IsAny<Exception>());

            var action = async () => await _service.Object.GetTransferHistoryAsync(It.IsAny<CancellationToken>());

            action.Should().ThrowExactlyAsync<Exception>();
        }
    }
}