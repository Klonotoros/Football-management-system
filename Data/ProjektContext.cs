using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Projekt.Models;

namespace Projekt.Data
{
    public class ProjektContext : DbContext
    {
        public ProjektContext(DbContextOptions<ProjektContext> options) : base(options) { }

        public DbSet<Player> Player { get; set; }
        public DbSet<Match> Match { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<MatchTeamCombination> MatchTeamCombination { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.TeamId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<MatchTeamCombination>()
                .HasOne(m => m.Team)
                .WithMany(t => t.MatchTeamCombinations)
                .HasForeignKey(m => m.TeamId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MatchTeamCombination>()
                .HasOne(m => m.Match)
                .WithMany(t => t.MatchTeamCombinations)
                .HasForeignKey(m => m.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MatchTeamCombination>()
                .HasKey(m => new { m.MatchId, m.TeamId });

            modelBuilder.Entity<Team>()
                .Property(x => x.Points).IsRequired(false);
        }
    }
}
