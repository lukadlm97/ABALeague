using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Domain.Entities
{
    public class LeagueGameLength
    {
        public virtual League League { get; set; }
        public int LeagueId { get; set; }
        public virtual GameLength GameLength { get; set; }
        public short GameLengthId { get; set; }
        [NotMapped]
        public GameLengthEnum GameLengthIdEnum => (GameLengthEnum) GameLengthId;
    }
}
