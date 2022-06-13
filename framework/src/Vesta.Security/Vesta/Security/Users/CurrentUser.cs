using Ardalis.GuardClauses;
using System.Linq;
using System.Security.Claims;
using Vesta.Security.Claims;

namespace Vesta.Security.Users
{
    public class CurrentUser : ICurrentUser
    {
        private readonly ICurrentPrincipalAccessor _currentPrincipal;

        public Guid? Id => _currentPrincipal.Principal?.FindUserId();

        public string Name => GetValue(ClaimTypes.Name);

        public string Email => GetValue(ClaimTypes.Email);

        public CurrentUser(ICurrentPrincipalAccessor currentPrincipal)
        {
            _currentPrincipal = currentPrincipal;
        }

        private string GetValue(string claimType)
        {
            Guard.Against.NullOrWhiteSpace(claimType);

            var claim = _currentPrincipal.Principal?.FindFirst(c => c.Type == claimType);
            if (!(claim is null))
            {
                return claim.Value;
            }

            return null;
        }

    }
}
