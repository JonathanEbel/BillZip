
using System;

namespace Identity.Models
{
    public class ApplicationUserClaim
    {
        public Guid id { get; set; }
        public string claimKey { get; set; }
        public string claimValue { get; set; }
    }
}
