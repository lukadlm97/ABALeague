using Microsoft.AspNetCore.Mvc.Rendering;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class PlayersByCountryViewModel
    {
        public string SelectedCountryId { get; set; }
        public SelectList Countries { get; set; }
        public IList<PlayerByCountryViewModel> PlayerItems { get; set; }
    }
}
