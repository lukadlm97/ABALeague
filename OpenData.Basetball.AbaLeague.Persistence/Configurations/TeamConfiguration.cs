using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basetball.AbaLeague.Persistence.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasData(new List<Team>()
            {
                new Team()
                {
                    Id = 1,
                    CountryId = 1,
            
                    Name = "Partizan",
                    ShortCode = "PAR",
                    RosterItems = new List<RosterItem>(),
                    CreatedBy = "Sys",
                    UpdateBy = "Sys",
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    UpdatedDate = DateTime.UtcNow.AddMinutes(-120),
                    
                }, new Team()
                {
                    Id = 2,
                    CountryId = 2,
          
                    Name = "Igokea",
                    ShortCode = "IGO",
                    RosterItems = new List<RosterItem>(),
                    CreatedBy = "Sys",
                    UpdateBy = "Sys",
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    UpdatedDate = DateTime.UtcNow.AddMinutes(-120),
                }
            });
        }
    }
}
