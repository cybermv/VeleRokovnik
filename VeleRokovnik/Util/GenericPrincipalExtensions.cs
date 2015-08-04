using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace System.Security.Principal
{
    public static class GenericPrincipalExtensions
    {
        public enum CustomOsobaClaims
        {
            OsobaId,
            OsobaIme,
            OsobaPrezime,
            OsobaNazivRokovnika,
            OsobaOpisRokovnika
        }

        public static string GetClaim(this IPrincipal user, CustomOsobaClaims claimKey)
        {
            if (!user.Identity.IsAuthenticated)
                return "";

            ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;
            Claim foundClaim = claimsIdentity.Claims.SingleOrDefault(claim => claim.Type == claimKey.ToString());

            return foundClaim != null ? foundClaim.Value : "";
        }

        public static int OsobaId(this IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return -1;

            ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;
            foreach (var claim in claimsIdentity.Claims)
            {
                if (claim.Type == "OsobaId")
                    return Convert.ToInt32(claim.Value);
            }
            return -1;
        }
    }
}