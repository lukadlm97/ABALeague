using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Persistence.Configurations
{
    public class RosterItemConfiguration : IEntityTypeConfiguration<RosterItem>
    {
        public void Configure(EntityTypeBuilder<RosterItem> builder)
        {
        }
    }
}
