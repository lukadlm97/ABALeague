﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basketball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Persistence.Configurations
{
    public class LeagueGameLengthConfiguration : IEntityTypeConfiguration<LeagueGameLength>
    {
        public void Configure(EntityTypeBuilder<LeagueGameLength> builder)
        {
            builder.HasKey(x => (new { x.GameLengthId, x.LeagueId }));
        }
    }
}
