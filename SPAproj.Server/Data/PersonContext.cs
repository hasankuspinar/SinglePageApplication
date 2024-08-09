using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SPAproj.Server;
using SPAproj.Server.Models;

namespace SPAproj.Server.Data
{
    public partial class PersonContext : DbContext
    {
        public PersonContext (DbContextOptions<PersonContext> options)
            : base(options)
        {
        }
        public DbSet<Person> Person { get; set; } = default!;
    }
}
