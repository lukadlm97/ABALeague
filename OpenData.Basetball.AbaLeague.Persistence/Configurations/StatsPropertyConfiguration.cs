using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Persistence.Configurations
{
    public class StatsPropertyConfiguration : IEntityTypeConfiguration<StatsProperty>
    {
        public void Configure(EntityTypeBuilder<StatsProperty> builder)
        {
            builder
                 .HasData(Enum.GetValues(typeof(StatsPropertyEnum))
                    .Cast<StatsPropertyEnum > ()
                    .Select(e =>
                        new StatsProperty()
                        {
                            Id = (short)e,
                            Name = e.ToString()
                        }));
        }
    }
}
