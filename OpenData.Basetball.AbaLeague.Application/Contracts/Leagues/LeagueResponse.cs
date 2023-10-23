using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Contracts.Leagues
{
    public class LeagueResponse
    {
        public int Id { get; set; }
        public string OfficalName { get; set; }
        public string ShortName { get; set; }
        public string Season { get; set; }
        public string StandingUrl { get; set; }
        public string CalendarUrl { get; set; }
        public string MatchUrl { get; set; }
        public string BoxScoreUrl { get; set; }
        public string? RosterUrl { get; set; }
        public string? BaseUrl { get; set; }
    }
}
