using System.ComponentModel.DataAnnotations;
using OpenData.Basetball.AbaLeague.Domain.Common;

namespace OpenData.Basetball.AbaLeague.Domain.Entities
{
    public class Country:BaseEntity
    {
        [Required, MaxLength(125)]
        public string Name { get; set; }

        public string CodeIso { get; set; }
        public string CodeIso2 { get; set; }
        public string Nationality { get; set; }
        public string? AbaLeagueCode { get; set; } = null;
    }
}
