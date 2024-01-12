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
    public class CompetitionOrganizationConfiguration : IEntityTypeConfiguration<CompetitionOrganization>
    {
        public void Configure(EntityTypeBuilder<CompetitionOrganization> builder)
        {
            builder
                 .HasData(Enum.GetValues(typeof(CompetitionOrganizationEnum))
                    .Cast<CompetitionOrganizationEnum>()
                    .Select(e =>
                        new CompetitionOrganization()
                        {
                            Id = (short)e,
                            Name = e.ToString()
                        }));
        }
    }
}
