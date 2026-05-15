namespace CompanyName.AspNetCore.Abstractions.Http
{
    public interface IHttpRequestBuilder
    {
        IHttpRequestBuilder WithMethod(HttpMethod method);

        IHttpRequestBuilder WithUri(Uri uri);

        HttpRequestMessage Build();
    }
}

