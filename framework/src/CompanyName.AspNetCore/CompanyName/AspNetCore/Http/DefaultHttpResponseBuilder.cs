using CommunityToolkit.Diagnostics;
using CompanyName.AspNetCore.Abstractions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace CompanyName.AspNetCore.Http
{
    public class DefaultHttpResponseBuilder : IHttpResponseBuilder, IErrorResponseBuilder
    {

        public static ProblemDetails DefaultProblemsDetails { get; protected set; }

        static DefaultHttpResponseBuilder()
        {
            DefaultProblemsDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred while processing the request.",
                Detail = "No additional details are available."
            };
        }


        protected ILogger Logger { get; set; }

        protected HttpContext HttpContext { get; }


        private byte[] _content;

        private int _statusCode;

        private string _contentType;

        private ProblemDetails _problemDetails;

        private bool _isProblem;

        private long _contentLength;


        public DefaultHttpResponseBuilder(IHttpContextAccessor httpContextAccessor, int defaultHttpStatusCode = StatusCodes.Status500InternalServerError)
        {
            Guard.IsNotNull(httpContextAccessor);

            HttpContext = httpContextAccessor.HttpContext;
            Logger = HttpContext.RequestServices.GetService<ILogger<DefaultHttpResponseBuilder>>() ?? NullLogger<DefaultHttpResponseBuilder>.Instance;

            _contentType = MediaTypeNames.Application.Json;
            _statusCode = defaultHttpStatusCode;
            _isProblem = false;

        }

        public IHttpResponseBuilder WithStatusCode(int statusCode)
        {
            _statusCode = statusCode;

            return this;
        }

        public IHttpResponseBuilder WithContentType(string contentType)
        {
            _contentType = contentType;

            return this;
        }

        public IHttpResponseBuilder WithContent(byte[] content)
        {

            if (content.HasContent())
            {
                _content = content;
            }

            return this;
        }

        public IHttpResponseBuilder WithContent(string content)
        {
            if (content is not null)
            {
                _content = Encoding.UTF8.GetBytes(content);
            }

            return this;
        }

        public IHttpResponseBuilder WithContent<T>(T content)
        {
            if (content is not null)
            {
                _content = content.ToByteArray();
            }

            return this;
        }

        public IErrorResponseBuilder AsProblem()
        {
            _isProblem = true;

            HttpContext.Response.ContentType = MediaTypeNames.Application.ProblemJson;

            return this;
        }

        public IErrorResponseBuilder WithProblemDetails(ProblemDetails details)
        {
            _problemDetails = details;

            return this;
        }

        public IErrorResponseBuilder WithException(Exception exception)
        {
            _problemDetails = new ProblemDetails
            {
                Status = _statusCode,
                Title = "Upstream request failed",
                Detail = exception?.Message
            };

            return this;
        }

        public virtual async Task WriteAsync(CancellationToken cancellationToken = default)
        {
            if (HttpContext.Response.HasStarted)
            {
                Logger.LogWarning("Cannot write response: response has already started.");
                return;
            }

            if (_isProblem)
            {
                _problemDetails ??= DefaultProblemsDetails;

                NormalizeStatusCode(_problemDetails);

                HttpContext.Response.StatusCode = _statusCode;

                await HttpContext.Response.WriteAsJsonAsync(_problemDetails, cancellationToken);
            }
            else
            {
                HttpContext.Response.StatusCode = _statusCode;

                HttpContext.Response.ContentType = _contentType;

                if (_content.HasContent())
                {
                    HttpContext.Response.ContentLength = _content.Length;

                    await HttpContext.Response.Body.WriteAsync(_content, 0, _content.Length, cancellationToken);
                }
                else
                {
                    Logger.LogWarning("No content or problem details provided for response.");
                }
            }
        }

        protected virtual int NormalizeStatusCode(ProblemDetails problemDetails)
        {
            return StatusCodes.Status500InternalServerError;
        }
    }
}

