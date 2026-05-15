using System.Security.Claims;

namespace CompanyName.Security.Claims
{
    public class ClaimTypeOptions
    {
        public string UserId { get; set; } = ClaimTypes.NameIdentifier;

        public string Name { get; set; } = ClaimTypes.Name;

        public string Email { get; set; } = ClaimTypes.Email;

        public string Role { get; set; } = ClaimTypes.Role;
    }
}
