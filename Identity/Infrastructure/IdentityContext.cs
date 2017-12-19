using Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
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

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.claims)
                .WithOne()
                .HasForeignKey(k => k.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }


    }
}
