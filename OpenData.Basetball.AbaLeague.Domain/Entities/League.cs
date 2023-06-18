using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Domain.Common;
using OpenData.Basketball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Domain.Entities
{
    public class League : AuditEntity
    {
        public string OfficalName { get; set; }
        public string ShortName { get; set; }
        public string Season { get; set; }
       // public string TeamParticipansUrl { get; set; }
        public string StandingUrl { get; set; }
        public string CalendarUrl { get; set; }
        public string MatchUrl { get; set; }
        public string? BaseUrl { get; set; }

        public ICollection<RoundMatch> RoundMatches { get; set; }

    }
}
