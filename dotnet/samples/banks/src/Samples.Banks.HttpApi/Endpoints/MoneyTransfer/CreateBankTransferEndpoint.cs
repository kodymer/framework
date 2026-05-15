using CompanyName.AspNetCore.Abstractions.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Samples.Banks.MoneyTransfers;

namespace Samples.Banks.Endpoints.MoneyTransfer
{
    public class CreateBankTransferEndpoint : IEndpoint
    {
        public void Map(IEndpointRouteBuilder app)
        {
            app
                .MapPost(
                    "api/m/bank/transfers",
                    async ([FromBody] CreateBankTransferCommand command, [FromServices] IValidator<CreateBankTransferCommand> validator,/* [FromServices] IScopedMediator mediator,*/ CancellationToken cancellationToken = default) =>
                    {
                        var validationResult = await validator.ValidateAsync(command, cancellationToken);
                        if (!validationResult.IsValid)
                        {
                            return Results.ValidationProblem(validationResult.ToDictionary());
                        }

                        //var response = await mediator
                        //    .CreateRequestClient<CreateBankTransferEndpoint>()
                        //    .GetResponse<Result<BankTransferDto>>(command, cancellationToken);

                        //return response.Message.IsSuccess ?
                        //    Results.Ok() :
                        //    Results.Problem(response.Message.ToProblemDetails());

                        return Results.Ok();

                    }
                 )
                .WithName("CreateBankTransfer")
                .WithSummary("Make a bank transfer.")
                .WithTags("Bank Transfers - Minimal APIs");
        }
    }
}
