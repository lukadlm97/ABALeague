using OpenData.Basetball.AbaLeague.Domain.Common;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Domain.Entities
{
    public class ResourceSelector : BaseEntity
    {
        public virtual League? League { get; set; }
        public int? LeagueId { get; set; }
        public string? Value { get; set; }

        public short HtmlQuerySelectorId { get; set; }
        [NotMapped]
        public HtmlQuerySelectorEnum HtmlQuerySelectorEnum => (HtmlQuerySelectorEnum) HtmlQuerySelectorId;
    }
}
