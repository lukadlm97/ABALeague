using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Persistence
{
    public sealed class AbaLeagueDbContext : AuditableDbContext
    {
        public AbaLeagueDbContext(DbContextOptions contextOptions) : base(contextOptions)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<SeasonResources> SeasonResources { get; set; }
        public DbSet<RosterItem> RosterItems { get; set; }
        public DbSet<RoundMatch> RoundMatches { get; set; }
  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AbaLeagueDbContext).Assembly);
        }
    }
}
