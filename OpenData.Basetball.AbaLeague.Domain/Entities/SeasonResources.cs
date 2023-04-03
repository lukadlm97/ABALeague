using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basetball.AbaLeague.Domain.Entities
{
    public class SeasonResources
    {
        public string TeamSourceUrl { get; set; }
        public virtual Team Team { get; set; }
        public int TeamId { get; set; }
        public virtual League League { get; set; } 
        public int LeagueId { get; set; }
    }
}
