using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Persistence.Configurations
{
    internal class SeasonResourceConfiguration : IEntityTypeConfiguration<SeasonResources>
    {
        public void Configure(EntityTypeBuilder<SeasonResources> builder)
        {
            builder.HasKey(x=>(new {x.TeamId,x.LeagueId}));
          
        }
    }
}
