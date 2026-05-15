using System.Text;
using CommunityToolkit.Diagnostics;

namespace System.Security.Claims
{
    public static class ClaimsPrincipalFormatter
    {
        public static string Serialize(ClaimsPrincipal principal)
        {
            Guard.IsNotNull(principal);

            using (var stream = new MemoryStream())
            {
                using (var principalBinaryWriter = new BinaryWriter(stream))
                {
                    principal.WriteTo(principalBinaryWriter);
                }

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static ClaimsPrincipal Deserialize(string principalSerialized)
        {
            Guard.IsNotNullOrEmpty(principalSerialized);

            var principalBinaryData = Encoding.UTF8.GetBytes(principalSerialized);
            using (var stream = new MemoryStream(principalBinaryData))
            {
                using (var princiapalBinaryReader = new BinaryReader(stream))
                {
                    return new ClaimsPrincipal(princiapalBinaryReader);
                }
            }
        }
    }
}
