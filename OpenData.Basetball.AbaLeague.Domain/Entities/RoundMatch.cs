using OpenData.Basetball.AbaLeague.Domain.Common;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Domain.Entities
{
    public class RoundMatch : AuditEntity
    {
        public int? HomeTeamId { get; set; }
        public virtual Team? HomeTeam { get; set; }
        public int? AwayTeamId { get; set; }
        public virtual Team? AwayTeam { get; set; }
        public int Round { get; set; }
        public int MatchNo { get; set; }
        public DateTime DateTime { get; set; }
    }
}
