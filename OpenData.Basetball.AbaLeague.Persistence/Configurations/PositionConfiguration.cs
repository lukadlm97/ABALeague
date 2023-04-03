using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using System.Reflection.Emit;

namespace OpenData.Basetball.AbaLeague.Persistence.Configurations
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder
                 .HasData(Enum.GetValues(typeof(PositionEnum))
                    .Cast<PositionEnum>()
                    .Select(e =>
                        new Position()
                        {
                            Id = (short)e,
                            Name = e.ToString()
                        }));
        }
    }
}
