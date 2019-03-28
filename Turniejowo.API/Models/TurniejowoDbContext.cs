using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Turniejowo.API.Models
{
    public class TurniejowoDbContext : DbContext
    {
        public TurniejowoDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Discipline> Disciplines{ get; set; }
        public DbSet<Tournament> Tournaments{ get; set; }
        public DbSet<Team> Teams{ get; set; }
        public DbSet<User> Users{ get; set; }
        public DbSet<Player> Players{ get; set; }
    }
}
