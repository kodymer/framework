using System.Security.Claims;

namespace CompanyName.Security.Claims
{
    public abstract class CurrentPrincipalAccessor : ICurrentPrincipalAccessor
    {
        public ClaimsPrincipal Principal => GetClaimsPrincipal();

        protected abstract ClaimsPrincipal GetClaimsPrincipal();
    }
}