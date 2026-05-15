using Microsoft.AspNetCore.Mvc;

namespace CompanyName.AspNetCore.Abstractions.Http
{
    public interface IErrorResponseBuilder
    {

        IErrorResponseBuilder WithProblemDetails(ProblemDetails details);

        IErrorResponseBuilder WithException(Exception exception);

        Task WriteAsync(CancellationToken cancellationToken = default);
    }
}

