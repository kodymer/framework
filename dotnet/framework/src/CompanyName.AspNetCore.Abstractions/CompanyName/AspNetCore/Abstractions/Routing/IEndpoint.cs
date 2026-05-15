using Microsoft.AspNetCore.Routing;

namespace CompanyName.AspNetCore.Abstractions.Routing
{
    public interface IEndpoint
    {
        void Map(IEndpointRouteBuilder app);
    }
}
