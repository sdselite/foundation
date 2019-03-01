using SDSFoundation.Model.Security.Enumerations;
using System.Security.Claims;

namespace SDSFoundation.ExtensionMethods.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool HasClaim(this ClaimsPrincipal principal, ClaimType type, string value)
        {
            return principal.HasClaim(type.ToString(), value);
        }

        public static bool HasClaim(this ClaimsPrincipal principal, string name, string value)
        {
            return principal.HasClaim(name, value);
        }
    }
}
