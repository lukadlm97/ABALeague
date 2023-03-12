using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Domain.Models;

namespace OpenData.Basetball.AbaLeague.Persistence
{
    public sealed class AbaLeagueDbContext:DbContext
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
  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AbaLeagueDbContext).Assembly);
            modelBuilder.Entity<Position>()
                .HasData(Enum.GetValues(typeof(PositionEnum))
                    .Cast<PositionEnum>()
                    .Select(e => 
                        new Position()
                        {
                            Id = (short)e,
                            Name = e.ToString()
                        }));
            modelBuilder.Entity<Domain.Models.SeasonResources>().HasNoKey();
        }
    }
}
