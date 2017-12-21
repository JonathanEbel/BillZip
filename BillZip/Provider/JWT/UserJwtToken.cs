using Identity.Models;
using System.Collections.Generic;

namespace BillZip.Provider.JWT
{
    public static class UserJwtToken
    {
        public static string tokenIssuer = "BillZip.Security.Bearer";
        public static string tokenAudience = "BillZip.Security.Bearer";
        public static string secretKey = "Test-secret-key-1234";


        public static string GetToken(string userName, List<ApplicationUserClaim> claims, int expirationInMinutes)
        {
            var token = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create(secretKey))
                                .AddSubject(userName)
                                .AddIssuer(tokenIssuer)
                                .AddAudience(tokenAudience)
                                .AddExpiry(expirationInMinutes);

            foreach (var claim in claims)
            {
                token.AddClaim(claim.claimKey, claim.claimValue);
            }

            var result = token.Build().Value;
            return result;
        }
    }
}
