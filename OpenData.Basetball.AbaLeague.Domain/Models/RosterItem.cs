using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basetball.AbaLeague.Domain.Models
{
    public class RosterItem
    {
        [Key]
        public int Id { get; set; }

        public virtual Player Player { get; set; }
        public virtual League League { get; set; }
        public int PlayerId { get; set; }
        public int LeagueId { get; set; }
        public DateTime DateOfInsertion { get; set; }
        public DateTime? EndOfActivePeriod { get; set; } = default;
    }
}
