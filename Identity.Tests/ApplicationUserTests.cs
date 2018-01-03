using System; 
using Xunit;
using Core.Cryptography;

namespace Identity.Tests
{
    public class ApplicationUserTests
    {
        [Fact]
        public void EmailAddressValidated()
        {
            Assert.Throws<FormatException>(() => new Models.ApplicationUser("jebel.com", "password", "password"));
            Assert.Throws<FormatException>(() => new Models.ApplicationUser("jebel@comsdf", "password", "password"));
            Assert.Throws<FormatException>(() => new Models.ApplicationUser("jebel@", "password", "password"));

            var newUser  = new Models.ApplicationUser("jebel@hello.com", "password", "password");
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
