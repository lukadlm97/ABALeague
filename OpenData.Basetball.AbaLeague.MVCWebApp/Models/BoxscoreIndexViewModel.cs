using System.Collections;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class BoxscoreIndexViewModel
    {
        public int LeagueId { get; set; }
        public IList<int> AvailableRounds { get; set; }
    }
}
