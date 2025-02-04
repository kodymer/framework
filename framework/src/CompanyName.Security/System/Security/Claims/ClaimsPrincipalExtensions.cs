using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {

        public static Guid? FindUserId(this ClaimsPrincipal principal)
        {
            var claim = principal?.FindFirst(ClaimTypes.NameIdentifier);
            if (claim is null)
            {
                return null;
            }

            if (Guid.TryParse(claim.Value, out var id))
            {
                return id;
            }

            return null;
        }
    }
}
