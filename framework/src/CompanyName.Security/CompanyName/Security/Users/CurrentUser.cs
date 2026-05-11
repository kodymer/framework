using CommunityToolkit.Diagnostics;
using CompanyName.Security.Claims;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;

namespace CompanyName.Security.Users
{
    public class CurrentUser : ICurrentUser
    {

        private readonly ClaimTypeOptions _options;

        private readonly ICurrentPrincipalAccessor _currentPrincipal;

        public virtual object Id => _currentPrincipal.Principal?.FindUserId(_options?.UserId);

        public virtual string Name => GetValue(_options?.Name ?? ClaimTypes.Name);

        public virtual string Role => GetValue(_options?.Role ?? ClaimTypes.Role);

        public virtual string Email => GetValue(_options?.Email ?? ClaimTypes.Email);


        public CurrentUser(ICurrentPrincipalAccessor currentPrincipal, IOptions<ClaimTypeOptions> options = null)
        {
            Guard.IsNotNull(currentPrincipal);

            _currentPrincipal = currentPrincipal;
            _options = options.Value;
        }

        public T GetId<T>()
            where T : struct, IParsable<T>
        {
            if (TryGetId<T>(out var id))
            {
                return id;
            }
            else
            {
                throw new InvalidCastException($"Cannot convert user id '{Id}' to type '{typeof(T).FullName}'.");
            }
        }

        public bool TryGetId<T>(out T id)
            where T : struct, IParsable<T>
        {
            return T.TryParse(Id?.ToString(), CultureInfo.InvariantCulture, out id);
        }

        private string GetValue(string claimType)
        {
            Guard.IsNotNullOrWhiteSpace(claimType);

            var claim = _currentPrincipal.Principal?.FindFirst(c => c.Type == claimType);
            if (!(claim is null))
            {
                return claim.Value;
            }

            return null;
        }
    }
}

