using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.ProjectName.Extensions
{
    public static class ResultExtensions
    {
        public static ProblemDetails ToProblemDetails(
            this IResultBase result,
            int statusCode = StatusCodes.Status400BadRequest,
            string title = "An error occurred while processing the request",
            string type = "https://httpstatuses.com/400",
            string instance = null)
        {
            var problemDetails = new ProblemDetails
            {
                Title = title,
                Detail = result.Errors.Count == 1
                    ? result.Errors[0].Message
                    : "Multiple errors occurred.",
                Status = statusCode,
                Type = type,
                Instance = instance
            };

            // Add all errors to the extensions
            problemDetails.Extensions["errors"] = result.Errors
                .Select(e => new
                {
                    message = e.Message,
                    metadata = e.Metadata
                }).ToList();

            return problemDetails;
        }
    }
}
