using System.ComponentModel.DataAnnotations;
using OpenData.Basetball.AbaLeague.Domain.Common;

namespace OpenData.Basetball.AbaLeague.Domain.Entities
{
    public class Team:AuditEntity
    {
      
        public string Name { get; set; }
        public string ShortCode { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public int Capacity { get; set; }
        public virtual Country? Country { get; set; }
        public int? CountryId { get; set; }
        public ICollection<RosterItem> RosterItems { get; set; }
    }
}
