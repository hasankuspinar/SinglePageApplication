using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SPAproj.Server;

namespace SPAproj.Server.Data
{
    public partial class PersonContext : DbContext
    {
        public PersonContext (DbContextOptions<PersonContext> options)
            : base(options)
        {
        }
        public DbSet<SPAproj.Server.Person> Person { get; set; } = default!;
    }
}
