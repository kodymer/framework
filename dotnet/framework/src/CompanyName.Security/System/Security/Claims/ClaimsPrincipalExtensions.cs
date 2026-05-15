using System.Globalization;

namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {

        public static object FindUserId(this ClaimsPrincipal principal, string claimType = ClaimTypes.NameIdentifier)
        {
            var value = principal?.FindFirst(claimType)?.Value;
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (Guid.TryParse(value, out var g))
            {
                return g;
            }

            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i))
            {
                return i;
            }

            if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var l))
            {
                return l;
            }

            return value;
        }

    }
}
