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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasOne(e => e.GuestTeam)
                      .WithMany(t => t.GuestMatches)
                      .HasForeignKey(k => k.GuestTeamId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Match_HomeTeam");

                entity.HasOne(e => e.HomeTeam)
                      .WithMany(t => t.HomeMatches)
                      .HasForeignKey(k => k.HomeTeamId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Match_GuestTeam");
            });

            modelBuilder.Entity<Points>(entity =>
            {
                entity.HasOne(e => e.Tournament)
                    .WithMany(t => t.TournamentPoints)
                    .HasForeignKey(p => p.TournamentId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Points_Tournament");

                entity.HasOne(e => e.Match)
                    .WithMany(m => m.MatchPoints)
                    .HasForeignKey(p => p.MatchId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Points_Match");

                entity.HasOne(e => e.Player)
                    .WithMany(p => p.PlayerPoints)
                    .HasForeignKey(p => p.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Points_Player");
            });
        }

        public DbSet<Discipline> Disciplines{ get; set; }
        public DbSet<Tournament> Tournaments{ get; set; }
        public DbSet<Team> Teams{ get; set; }
        public DbSet<User> Users{ get; set; }
        public DbSet<Player> Players{ get; set; }
        public DbSet<Match> Matches{ get; set; }
        public DbSet<Points> Points { get; set; }
    }
}
