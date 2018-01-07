using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Core.Cryptography;

namespace Identity.Models
{
    public class ApplicationUser
    {
        public Guid Id { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string Salt { get; private set; }
        public DateTime LastLogin { get; private set; }
        public DateTime DateCreated { get; private set; }
        public List<ApplicationUserClaim> Claims { get; private set; }


        public ApplicationUser()
        { }

        public ApplicationUser(string userName, string password, string confirmPassword)
        {
            if (confirmPassword != password)
                throw new ArgumentException("Password and Confirm Password parameters don't match");

            SetEmailAddressUserName(userName);
            Id = Guid.NewGuid();

            SetSaltAndPassword(password);

            DateCreated = DateTime.Now;
        }


        public void UpdateLastLogin()
        {
            LastLogin = DateTime.Now;
        }


        public void AddClaim(string key, string value)
        {
            if (Claims == null)
                Claims = new List<ApplicationUserClaim>();

            Claims.Add(new ApplicationUserClaim
            {
                claimKey = key,
                claimValue = value
            });
        }

        public void ClearClaims()
        {
            if (Claims != null)
                Claims.Clear();
        }

        public void OverwriteUserName(string userName)
        {
            SetEmailAddressUserName(userName);
        }

        private void SetEmailAddressUserName(string userName)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(userName);
            if (match.Success)
                UserName = userName;
            else
                throw new FormatException("Email/Username formatted incorrectly.");

        }

        private void SetSaltAndPassword(string password)
        {
            //one way hash the password and store both the Salt and the hashed password
            Salt = Crypto.getSalt();
            Password = Crypto.getHash(password + Salt);
        }

        public void UpdatePassword(string password, string confirmPassword)
        {
            if (confirmPassword != password)
                throw new ArgumentException("Password and Confirm Password parameters don't match");

            SetSaltAndPassword(password);
        }

    }

   
}
