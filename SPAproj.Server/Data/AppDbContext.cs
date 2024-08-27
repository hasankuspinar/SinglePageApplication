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
    public DbSet<User> User { get; set; }
    public DbSet<UserPassword> UserPassword { get; set; }
    public DbSet<UserRole> UserRole { get; set; }
    public DbSet<UserStatus> UserStatus { get; set; }
    public DbSet<ConfigurationParameters> ConfigurationParameters { get; set; }


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
}
