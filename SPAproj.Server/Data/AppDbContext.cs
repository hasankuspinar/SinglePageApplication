using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SPAproj.Server.Models;

namespace SPAproj.Server.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserPassword> UserPasswords { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserStatus> UserStatuses { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseInMemoryDatabase("TestDatabase");
        }
    }
}
