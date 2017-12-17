using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<ApplicationUserClaim> claims { get; set; }
    }
}
