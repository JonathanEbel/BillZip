using Identity.Infrastructure;
using Identity.Infrastructure.Repos;
using Identity.Models;
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

            var user = new ApplicationUser("Admin@admin.com", "Password123", "Password123");
            user.AddClaim(Policies.Landlord.RequireClaim, Policies.Landlord.RequiredValues[0]);
            user.AddClaim(Policies.Admin.RequireClaim, "");
            userRepo.Add(user);
            userRepo.Save();
        }
    }
        
}
