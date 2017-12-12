using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Building_Management.Models
{
    public class BuildingManagementContext : DbContext
    {
        public BuildingManagementContext(DbContextOptions<BuildingManagementContext> options) : base(options)
        {

        }
        
        public DbSet<User> Users { get; set; }
    }
}
