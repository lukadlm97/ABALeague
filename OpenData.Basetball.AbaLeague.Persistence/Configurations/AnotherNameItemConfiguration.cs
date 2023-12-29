using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Persistence.Configurations
{
    internal class AnotherNameItemConfiguration : IEntityTypeConfiguration<AnotherNameItem>
    {
        public void Configure(EntityTypeBuilder<AnotherNameItem> builder)
        {
        }
    }
}
