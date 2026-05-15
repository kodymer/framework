namespace CompanyName.AspNetCore.Abstractions.Http
{
    public interface IHttpResponseBuilder
    {
        IHttpResponseBuilder WithStatusCode(int statusCode);

        IHttpResponseBuilder WithContentType(string contentType);

        IHttpResponseBuilder WithContent(byte[] content);

        IHttpResponseBuilder WithContent<T>(T content);

        IHttpResponseBuilder WithContent(string content);

        IErrorResponseBuilder AsProblem();

        Task WriteAsync(CancellationToken cancellationToken = default);
    }
}

