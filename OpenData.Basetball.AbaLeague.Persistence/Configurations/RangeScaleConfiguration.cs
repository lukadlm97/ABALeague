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
    public class RangeScaleConfiguration : IEntityTypeConfiguration<RangeScale>
    {
        public void Configure(EntityTypeBuilder<RangeScale> builder)
        {
         
        }
    }
}
