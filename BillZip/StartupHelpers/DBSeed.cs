using Identity.Infrastructure;
using Identity.Infrastructure.Repos;
using Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace BillZip.StartupHelpers
{
    public class DBSeed
    {   
        public static void Initialize(IdentityContext context, IApplicationUserRepository userRepo)
        {
            context.Database.EnsureCreated();

            // Look for any users.
            if (context.ApplicationUsers.Any())
            {
                return; // DB has been seeded
            }

            var claims = new List<ApplicationUserClaim>(){
                    new ApplicationUserClaim {
                        claimKey = Policies.Landlord.RequireClaim,
                        claimValue = Policies.Landlord.RequiredValues[0]
                    },
                    new ApplicationUserClaim {
                        claimKey = Policies.Admin.RequireClaim,
                        claimValue = ""
                    }
            };

            var user = new ApplicationUser("Admin@admin.com", "Password123", "Password123", claims);
            userRepo.Add(user);
            userRepo.Save();
        }
    }
        
}
