using Microsoft.AspNetCore.Mvc.Rendering;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class PlayerDraftViewModel
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Height { get; set; }
        public string SelectedCountryId { get; set; }
        public SelectList Countries { get; set; }
        public string SelectedPositionId { get; set; }
        public SelectList Positions { get; set; }
    }
}
