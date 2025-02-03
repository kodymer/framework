namespace Vesta.Banks.Options
{
    public class AuthenticationOptions
    {
        public const string SectionName = "Authentication";

        public string Authority { get; set; } = null!;

        public string Audience { get; set; } = null!;

        public string Scope { get; set; } = null!;

        public string SwaggerClientId { get; set; } = null!;
    }
}