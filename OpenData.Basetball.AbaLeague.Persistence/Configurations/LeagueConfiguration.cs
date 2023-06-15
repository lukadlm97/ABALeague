using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Persistence.Configurations
{
    public class LeagueConfiguration : IEntityTypeConfiguration<League>
    {
        public void Configure(EntityTypeBuilder<League> builder)
        {
            /*
            builder.HasData(new List<League>()
            {
                new League()
                {
                    Id = 1,
                    OfficalName = "NLB ABA League",
                    Season = "2022/23",
                    ShortName = "ABA",
                    StandingUrl = "ur1",
                    CreatedBy = "Sys",
                    UpdateBy = "Sys",
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    UpdatedDate = DateTime.UtcNow.AddMinutes(-120)
                },  new League()
                {
                    Id = 2,
                    OfficalName = "NLB ABA League 2",
                    Season = "2022/23",
                    ShortName = "ABA2",
                    StandingUrl = "ur1",
                    CreatedBy = "Sys",
                    UpdateBy = "Sys",
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    UpdatedDate = DateTime.UtcNow.AddMinutes(-120)
                },
            });
            */
        }
    }
}
