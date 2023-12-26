using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Persistence.Configurations
{
    public class ProcessorTypeConfiguration : IEntityTypeConfiguration<Basketball.AbaLeague.Domain.Entities.ProcessorType>
    {
        public void Configure(EntityTypeBuilder<ProcessorType> builder)
        {
            builder
                .HasData(Enum.GetValues(typeof(Basketball.AbaLeague.Domain.Enums.ProcessorType))
                   .Cast<Basketball.AbaLeague.Domain.Enums.ProcessorType>()
                   .Select(e =>
                       new Basketball.AbaLeague.Domain.Entities.ProcessorType()
                       {
                           Id = (short) e,
                           Name = e.ToString()
                       }));
        }
    }
}
