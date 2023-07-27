
namespace OpenData.Basetball.AbaLeague.Domain.Entities
{
    public class SeasonResources
    {
        public string TeamSourceUrl { get; set; }
        public virtual Team Team { get; set; }
        public int TeamId { get; set; }
        public virtual League League { get; set; }
        public int LeagueId { get; set; }
        public string TeamName { get; set; }
        public string TeamUrl { get; set; }
        public string? IncrowdUrl { get; set; }
    }
}
