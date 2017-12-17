using Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {

        }

        DbSet<ApplicationUserClaim> ApplicationUserClaims { get; set; }
        DbSet<ApplicationUser> ApplicationUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().HasKey(m => m.Id);
            builder.Entity<ApplicationUserClaim>().HasKey(m => m.id);
            base.OnModelCreating(builder);
        }


    }
}
