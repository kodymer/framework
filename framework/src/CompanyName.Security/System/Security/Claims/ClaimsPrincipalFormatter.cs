using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Security.Claims
{
    public static class ClaimsPrincipalFormatter
    {
        public static string Serialize(ClaimsPrincipal principal)
        {
            Guard.Against.Null(principal, nameof(principal));

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
            Guard.Against.NullOrEmpty(principalSerialized, nameof(principalSerialized));

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
