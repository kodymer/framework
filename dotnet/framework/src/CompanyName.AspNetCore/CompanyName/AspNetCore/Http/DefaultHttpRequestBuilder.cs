using CommunityToolkit.Diagnostics;
using CompanyName.AspNetCore.Abstractions.Http;
using Microsoft.AspNetCore.Http;

namespace CompanyName.AspNetCore.Http
{
    public class DefaultHttpRequestBuilder : IHttpRequestBuilder
    {

        public static HttpMethod DefaultRequestMethod => HttpMethod.Get;


        private readonly HttpContext HttpContext;

        private HttpMethod _method;

        private Uri _uri;


        public DefaultHttpRequestBuilder(IHttpContextAccessor httpContextAccessor)
        {
            Guard.IsNotNull(httpContextAccessor);

            HttpContext = httpContextAccessor.HttpContext;

            _method = DefaultRequestMethod;
        }

        public IHttpRequestBuilder WithMethod(HttpMethod method)
        {
            _method = method;

            return this;
        }

        public IHttpRequestBuilder WithUri(Uri uri)
        {
            _uri = uri;

            return this;
        }

        public HttpRequestMessage Build()
        {
            var request = new HttpRequestMessage()
            {
                Method = _method ?? DefaultRequestMethod,
                RequestUri = _uri
            };

            return request;
        }
    }
}

