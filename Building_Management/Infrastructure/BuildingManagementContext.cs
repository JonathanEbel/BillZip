using Building_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Building_Management.Infrastructure
{
    public class BuildingManagementContext : DbContext
    {
        public BuildingManagementContext(DbContextOptions<BuildingManagementContext> options) : base(options)
        {

        }
        
        public DbSet<User> Users { get; set; }
    }
}
