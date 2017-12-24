using System; 
using System.Collections.Generic;
using Xunit;
using Core.Cryptography;

namespace Identity.Tests
{
    public class ApplicationUserTests
    {
        [Fact]
        public void EmailAddressValidated()
        {
            var userClaims = new List<Models.ApplicationUserClaim>();
            userClaims.Add(new Models.ApplicationUserClaim
            {
                claimKey = "Test",
                claimValue = "yes"
            });

            Assert.Throws<FormatException>(() => new Models.ApplicationUser("jebel.com", "password", "password", userClaims));
            Assert.Throws<FormatException>(() => new Models.ApplicationUser("jebel@comsdf", "password", "password", userClaims));
            Assert.Throws<FormatException>(() => new Models.ApplicationUser("jebel@", "password", "password", userClaims));

            var newUser  = new Models.ApplicationUser("jebel@hello.com", "password", "password", userClaims);
            Assert.Equal("jebel@hello.com", newUser.UserName);
        }


        [Fact]
        public void PasswordHashingMatches()
        {
            var salt = Crypto.getSalt();
            var hashedPassword = Crypto.getHash("password" + salt);

            Assert.NotEqual("password" + salt, hashedPassword);
            Assert.Equal(hashedPassword, Crypto.getHash("password" + salt));

        }
    }
}
