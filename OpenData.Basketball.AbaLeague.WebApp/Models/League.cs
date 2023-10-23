using System.ComponentModel.DataAnnotations;

namespace OpenData.Basketball.AbaLeague.WebApp.Models
{
    public class League
    {
        [StringLength(120, MinimumLength = 5)]
        [Required]
        public string OfficalName { get; set; }
        [StringLength(10)]
        [Required]
        public string ShortName { get; set; }
        [Required]
        public string Season { get; set; }
        [Required]
        public string StandingUrl { get; set; }
        [Required]
        public string CalendarUrl { get; set; }
        [Required]
        public string MatchUrl { get; set; }
        [Required]
        public string BoxScoreUrl { get; set; }
        public string? RosterUrl { get; set; }
        public string? BaseUrl { get; set; }
    }
}
