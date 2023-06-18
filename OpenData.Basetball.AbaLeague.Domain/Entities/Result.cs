using OpenData.Basetball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Domain.Entities
{
    public class Result:AuditEntity
    {
        public int HomeTeamPoints { get; set; }
        public int AwayTeamPoint { get; set; }
        public int Attendency { get; set; }
        public string Venue { get; set; }
        public virtual RoundMatch RoundMatch { get; set; }
        public int RoundMatchId { get; set; }

    }
}
