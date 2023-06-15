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
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            /*
            builder.HasData(new List<Player>()
            {
                new Player()
                {
                    Id = 3,
                    PositionId = 2,
                    CountryId = 1,
                    Height = 193,
                    Name = "Dragan Milosavljevic",
                    DateOfBirth = DateTime.UtcNow.AddYears(-32),
                    Nationality = "Serbian",
                    CreatedBy = "Sys",
                    UpdateBy = "Sys",
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    UpdatedDate = DateTime.UtcNow.AddMinutes(-120)
                },  new Player()
                {
                    Id = 4,
                    PositionId = 5,
                    CountryId = 3,
                    Height = 210,
                    Name = "Miro Bilan",
                    DateOfBirth = DateTime.UtcNow.AddYears(-36),
                    Nationality = "Croatian",
                    CreatedBy = "Sys",
                    UpdateBy = "Sys",
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    UpdatedDate = DateTime.UtcNow.AddMinutes(-120)
                },new Player()
                {
                    Id = 5,
                    PositionId = 3,
                    CountryId = 1,
                    Height = 201,
                    Name = "Uros Trifunovic",
                    DateOfBirth = DateTime.UtcNow.AddYears(-21),
                    Nationality = "Serbian",
                    CreatedBy = "Sys",
                    UpdateBy = "Sys",
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    UpdatedDate = DateTime.UtcNow.AddMinutes(-120)
                },new Player()
                {
                    Id = 6,
                    PositionId = 1,
                    CountryId = 1,
                    Height = 210,
                    Name = "Nikola Topic",
                    DateOfBirth = DateTime.UtcNow.AddYears(-17),
                    Nationality = "Serbian",
                    CreatedBy = "Sys",
                    UpdateBy = "Sys",
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    UpdatedDate = DateTime.UtcNow.AddMinutes(-120)
                }
            });*/
        }
    }
}
