using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Domain.Enums;

namespace OpenData.Basketball.AbaLeague.Persistence.Configurations
{
    public class HtmlQuerySelectorConfiguration : IEntityTypeConfiguration<HtmlQuerySelector>
    {
        public void Configure(EntityTypeBuilder<HtmlQuerySelector> builder)
        {
            builder
                 .HasData(Enum.GetValues(typeof(HtmlQuerySelectorEnum))
                    .Cast<HtmlQuerySelectorEnum>()
                    .Select(e =>
                        new HtmlQuerySelector()
                        {
                            Id = (short)e,
                            Name = e.ToString()
                        }));
        }
    }
}
