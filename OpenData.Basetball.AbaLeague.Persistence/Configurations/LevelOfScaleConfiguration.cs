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
    public class LevelOfScaleConfiguration : IEntityTypeConfiguration<LevelOfScale>
    {
        public void Configure(EntityTypeBuilder<LevelOfScale> builder)
        {
            builder
                 .HasData(Enum.GetValues(typeof(LevelOfScaleEnum))
                    .Cast<LevelOfScaleEnum>()
                    .Select(e =>
                        new LevelOfScale()
                        {
                            Id = (short)e,
                            Name = e.ToString()
                        }));
        }
    }
}
