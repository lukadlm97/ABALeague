using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class LeagueItemViewModel
    {
        public int? Id { get; set; }
        public string OfficialName { get; set; }
        public string ShortName { get; set; }
        public string StandingUrl { get; set; }
        public string CalendarUrl { get; set; }
        public string MatchUrl { get; set; }
        public string BoxScoreUrl { get; set; }
        public string BaseUrl { get; set; }
        public string RosterUrl { get; set; }
        public string SelectedProcessorTypeId { get; set; }
        public SelectList ProcessorTypes { get; set; }
        public string SelectedSeasonId { get; set; }
        public SelectList Seasons { get; set; }
        public int RoundsToPlay { get; set; }
        public string SelectedCompetitionOrganizationId { get; set; }
        public SelectList CompetitionOrganizations { get; set; }
    }
}
