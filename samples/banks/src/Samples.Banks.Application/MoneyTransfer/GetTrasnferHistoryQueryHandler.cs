using CompanyName.Caching;
using CompanyName.Cqrs.Abstractions;
using FluentResults;
using Microsoft.Extensions.Caching.Distributed;
using Samples.Banks.Transfers;

namespace Samples.Banks.MoneyTransfers
{
    public class GetTrasnferHistoryQueryHandler : QueryHandler<GetTransferHistoryQuery, Result<List<BankTransferDto>>>
    {
        private readonly IDistributedCache _cache;
        private readonly IBankTransferRepository _repository;

        public GetTrasnferHistoryQueryHandler(
            IDistributedCache cache,
            IBankTransferRepository repository)
        {
            _cache = cache;
            _repository = repository;

            LocalizationResource = typeof(BanksResource);
        }

        public override async Task<Result<List<BankTransferDto>>> HandleAsync(GetTransferHistoryQuery query, CancellationToken cancellationToken = default)
        {
            List<BankTransferDto> dtos = null;

            try
            {
                Logger.GettingAllBankAccounts();

                var entities = await _cache.GetOrAddAsync(
                    "GetTransferHistory",
                    async () => await _repository.ListAsync(new BankTransfersOrderedByCreationTimeAscSpecification(), cancellationToken));

                Logger.BankAccountCountObtained(entities.Count);

                //
                //   Without cache:
                // 
                //   var entities = await _repository.ListAsync(new BankTransfersOrderedByCreationTimeAsc(), cancellationToken));
                //

                dtos = ObjectMapper.Map<List<BankTransferDto>>(entities);
                return Result.Ok(dtos);
            }
            catch (Exception e)
            {
                Logger.CouldNotGetBankAccounts(e);

                return Result.Fail(new Error("Error caused by exception.").CausedBy(e));
            }
        }
    }
}
