using CompanyName.AspNetCore.Abstractions.Routing;
using CompanyName.Ddd.Domain.Common.Pagination;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Samples.Banks.AccountManagement;

namespace Samples.Banks.Endpoints.AccountManagement
{
    public class GetBankAccountsEndpoint : IEndpoint
    {

        public void Map(IEndpointRouteBuilder app)
        {
            app
                .MapGet(
                    "api/m/bank/{branchId}/accounts",
                    async Task<Results<Ok<PagedResult<BankAccountDto>>, ProblemHttpResult>> (Guid branchId, [FromQuery] int? page, [FromQuery] int? pageSize/*, [FromServices] IScopedMediator mediator*/, CancellationToken cancellationToken = default) =>
                    {
                        var query = new GetAllBankAccountsQuery()
                        {
                            BranchId = branchId,
                            Page = page,
                            PageSize = pageSize
                        };

                        //var response = await mediator
                        //    .CreateRequestClient<GetAllBankAccountsQuery>()
                        //    .GetResponse<Result<PagedResult<BankAccountDto>>>(query);

                        //if (response.Message.IsSuccess)
                        //{
                        //    return TypedResults.Ok(response.Message.Value);
                        //}
                        //else
                        //{
                        //    return TypedResults.Problem(response.Message.ToProblemDetails());
                        //}

                        return TypedResults.Ok<PagedResult<BankAccountDto>>(null);
                    }
                 )
                .WithName("GetBankAccounts")
                .WithSummary("Get all bank accounts.")
                .WithTags("Bank Accounts - Minimal APIs");
        }
    }
}
