using Microsoft.AspNetCore.Mvc.Rendering;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class UpsertPlayerViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? PositionId { get; set; }
        public string SelectedCountryId { get; set; }
        public SelectList Countries { get; set; }

        public string SelectedPositionId { get; set; }
        public SelectList Positions { get; set; }
        public bool ComplexRouting { get; set; } = false;
        public int LeagueId { get; set; }

    }
}
