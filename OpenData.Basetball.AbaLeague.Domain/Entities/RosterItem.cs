using OpenData.Basetball.AbaLeague.Domain.Common;

namespace OpenData.Basetball.AbaLeague.Domain.Entities
{
    public class RosterItem:BaseEntity
    {

        public virtual Player Player { get; set; }
        public virtual League League { get; set; }
        public virtual Team Team { get; set; }
        public int PlayerId { get; set; }
        public int LeagueId { get; set; }
        public int TeamId { get; set; }
        public DateTime DateOfInsertion { get; set; }
        public DateTime? EndOfActivePeriod { get; set; } = default;
    }
}
