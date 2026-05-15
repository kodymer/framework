using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.Security.Claims
{
    public class ThreadCurrentPrincipalAccessor : CurrentPrincipalAccessor
    {
        protected override ClaimsPrincipal GetClaimsPrincipal()
        {
            return Thread.CurrentPrincipal as ClaimsPrincipal;
        }
    }
}
