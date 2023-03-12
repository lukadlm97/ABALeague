using System.ComponentModel.DataAnnotations;

namespace OpenData.Basetball.AbaLeague.Domain.Models
{
    public class Team
    {
        [Key]
        public int ID { get; set; }
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
