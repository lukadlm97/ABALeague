using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class SeasonResourcesViewModel
    {
        public int LeagueId { get; set; }
        public IList<AddSeasonResourceDraftDto> MissingTeams { get; set; }
        public IList<AddSeasonResourceDraftDto> DraftTeams { get; set; }
        public IList<AddSeasonResourceDraftDto> ExistingTeams { get; set; }
        public bool IsLeagueOrganization { get; set; }
    }
}
