using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Polly;
namespace Common.Utilities
{
    public static class ContextRolesHelper
    {
        public static ICollection<string> GetRoles(AuthorizationHandlerContext context)
        {
            // Fetch the realm_access claim
            var realmAccessClaim = context.User.FindFirst("realm_access")?.Value;

            // Parse roles from the claim
            List<string> roles = new List<string>();
            if (!string.IsNullOrEmpty(realmAccessClaim))
            {
                var parsedRealmAccess = JObject.Parse(realmAccessClaim);
                roles = parsedRealmAccess["roles"].ToObject<List<string>>();
            }
            return roles;
        }
    }
}
