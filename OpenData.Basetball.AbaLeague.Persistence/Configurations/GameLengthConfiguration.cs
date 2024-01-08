using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Domain.Enums;

namespace OpenData.Basketball.AbaLeague.Persistence.Configurations
{
    public class GameLengthConfiguration : IEntityTypeConfiguration<GameLength>
    {
        public void Configure(EntityTypeBuilder<GameLength> builder)
        {
            builder
                 .HasData(Enum.GetValues(typeof(GameLengthEnum))
                    .Cast<GameLengthEnum>()
                    .Select(e =>
                        new GameLength()
                        {
                            Id = (short)e,
                            Name = e.ToString()
                        }));
        }
    }
}
