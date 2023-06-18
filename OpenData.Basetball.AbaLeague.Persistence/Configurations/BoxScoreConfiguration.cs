using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenData.Basketball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Persistence.Configurations
{
  
    internal class BoxScoreConfiguration : IEntityTypeConfiguration<BoxScore>
    {
        public void Configure(EntityTypeBuilder<BoxScore> builder)
        {
            builder.HasKey(x => (new { x.RoundMatchId, x.RosterItemId }));

        }
    }
}
