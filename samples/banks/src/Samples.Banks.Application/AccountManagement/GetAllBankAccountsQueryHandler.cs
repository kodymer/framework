using CompanyName.Cqrs.Abstractions;
using CompanyName.Ddd.Domain.Common.Filters;
using CompanyName.Ddd.Domain.Common.Pagination;
using FluentResults;
using Microsoft.Extensions.Caching.Hybrid;
using Samples.Banks.Accounts;

namespace Samples.Banks.AccountManagement
{
    public class GetAllBankAccountsQueryHandler : QueryHandler<GetAllBankAccountsQuery, Result<PagedResult<BankAccountDto>>>
    {
        const string KeyTemplate = "BankAccounts-{0}-Page-{1}-PageSize-{2}";

        private readonly HybridCache _cache;
        private readonly IBankAccountRepository _repository;

        public GetAllBankAccountsQueryHandler(
            HybridCache cache,
            IBankAccountRepository repository)
        {
            _cache = cache;
            _repository = repository;

            LocalizationResource = typeof(BanksResource);
        }

        public override async Task<Result<PagedResult<BankAccountDto>>> HandleAsync(GetAllBankAccountsQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                Logger.GettingAllBankAccounts();

                //                 
                //  For best practices using hybrid cache, see
                //  https://medium.com/@serhatalftkn/understanding-hybridcache-in-net-9-79f589a59458              
                //  

                var tags = new[]
                {
                    "bankaccount",
                    $"branch:{query.BranchId}",
                };

                //  
                //  If LocalCacheExpiration is shorter than Expiration, the system 
                //  will first attempt to serve the value from local memory, and if 
                //  it's not available, it will fall back to Redis. If LocalCacheExpiration
                //  is not configured, the system might either skip using local cache 
                //  or retain the value indefinitely (depending on the implementation).
                //  If both are configured, they apply independently.

                var entryOptions = new HybridCacheEntryOptions()
                {
                    Expiration = TimeSpan.FromMinutes(30),           // Set cache expiration time (Redis).
                    LocalCacheExpiration = TimeSpan.FromSeconds(10), // Set local cache expiration time (MemoryCache).
                };

                var pagination = new PaginationFilter(query.Page, query.PageSize);
                var specification = new BankAccountsOrderedByNumberAscSpecification(query.BranchId);

                // TO-DO: review set entity private set property .
                var pagedResult = await _cache.GetOrCreateAsync(
                    string.Format(KeyTemplate, query.BranchId, query.Page, query.PageSize),
                    async (@cancellationToken) => await _repository.ProjectToPagedAsync<BankAccountDto>(
                        pagination,
                        specification,
                        @cancellationToken),
                    options: entryOptions,
                    tags: tags,
                    cancellationToken: cancellationToken);

                //
                // Without cache:
                // 
                // var entities = await _repository.ListAsync(new BankAccountsOrderedByNumberAscSpecification(), cancellationToken);
                //

                Logger.BankAccountCountObtained(pagedResult.Data.Count);

                return Result.Ok(pagedResult);
            }
            catch (Exception e)
            {
                Logger.CouldNotGetBankAccounts(e);

                return Result.Fail(new Error("Error caused by exception.").CausedBy(e));
            }
        }
    }
}
