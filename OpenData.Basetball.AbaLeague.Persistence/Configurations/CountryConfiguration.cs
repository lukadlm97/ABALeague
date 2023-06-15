using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Persistence.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            /*
            builder.HasData(new List<Country>()
            {
                new Country()
                {
                    Id = 1,
                    CodeIso = "SRB",
                    CodeIso2 = "RS",
                    Name = "Serbia"
                },
                new Country()
                {
                    Id = 2,
                    CodeIso = "BIH",
                    CodeIso2 = "BH",
                    Name = "Bosnia and Herzegovina"
                },
                new Country()
                {
                    Id = 3,
                    CodeIso = "CRO",
                    CodeIso2 = "HR",
                    Name = "Croatia"
                },
            });*/
        }
    }
}
