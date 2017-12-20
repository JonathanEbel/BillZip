using Identity.Models;
using System.Collections.Generic;

namespace BillZip.Provider.JWT
{
    public static class UserJwtToken
    {
        public static string GetToken(string userName, List<ApplicationUserClaim> claims)
        {
            var token = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create("Test-secret-key-1234"))
                                .AddSubject(userName)
                                .AddIssuer("BillZip.Security.Bearer")
                                .AddAudience("BillZip.Security.Bearer")
                                .AddExpiry(500);

            foreach (var claim in claims)
            {
                token.AddClaim(claim.claimKey, claim.claimValue);
            }

            var result = token.Build().Value;
            return result;
        }
    }
}
