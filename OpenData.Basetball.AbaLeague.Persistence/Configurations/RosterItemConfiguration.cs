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
    public class RosterItemConfiguration : IEntityTypeConfiguration<RosterItem>
    {
        public void Configure(EntityTypeBuilder<RosterItem> builder)
        {
        }
    }
}
