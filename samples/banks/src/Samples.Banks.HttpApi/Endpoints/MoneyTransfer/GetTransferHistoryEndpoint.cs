using CompanyName.AspNetCore.Abstractions.Routing;
using FluentResults;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Samples.Banks.MoneyTransfers;

namespace Samples.Banks.Endpoints.MoneyTransfer
{
    public class GetTransferHistoryEndpoint : IEndpoint
    {
        public void Map(IEndpointRouteBuilder app)
        {
            app
                .MapGet(
                    "api/m/bank/transfers",
                    async Task<Results<Ok<List<BankTransferDto>>, NotFound>> ([FromServices] IScopedMediator mediator) =>
                    {
                        var query = new GetTransferHistoryEndpoint();
                        var response = await mediator.CreateRequestClient<GetTransferHistoryEndpoint>().GetResponse<Result<List<BankTransferDto>>>(query);
                        if (response.Message.IsSuccess)
                        {
                            return TypedResults.Ok(response.Message.Value);
                        }
                        else
                        {
                            return TypedResults.NotFound();
                        }
                    }
                 )
                .WithName("GetTransferHistory")
                .WithSummary("Get all bank transfers.")
                .WithTags("Bank Transfers - Minimal APIs");
        }
    }
}
