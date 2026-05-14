using CompanyName.AspNetCore.Abstractions.Routing;
using FluentResults;
using FluentValidation;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Samples.Banks.AccountManagement;
using Samples.Banks.Extensions;

namespace Samples.Banks.Endpoints
{
    public class CreateBankAccountEndpoint : IEndpoint
    {
        public void Map(IEndpointRouteBuilder app)
        {
            app
            .MapPost(
                "/api/m/bank/accounts",
                async Task<Results<Ok, ProblemHttpResult, ValidationProblem>> ([FromBody] CreateBankAccountCommand command, [FromServices] IValidator<CreateBankAccountCommand> validator, [FromServices] IScopedMediator mediator, CancellationToken cancellationToken = default) =>
                {
                    var validationResult = await validator.ValidateAsync(command, cancellationToken);
                    if (!validationResult.IsValid)
                    {
                        return TypedResults.ValidationProblem(validationResult.ToDictionary());
                    }

                    Response<Result> response = null;

                    try
                    {
                        response = await mediator
                           .CreateRequestClient<CreateBankAccountCommand>()
                           .GetResponse<Result>(command, cancellationToken);

                        return response.Message.IsSuccess ?
                              TypedResults.Ok() :
                              TypedResults.Problem(response.Message.ToProblemDetails());
                    }
                    catch (Exception)
                    {
                                            
                    }

                    return TypedResults.Ok();
                }
             )
            .WithName("CreateBankAccount")
            .WithSummary("Create a new bank account.")
            .WithTags("Bank Accounts - Minimal APIs");
        }
    }
}
