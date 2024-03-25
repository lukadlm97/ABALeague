using Microsoft.AspNetCore.Mvc.Rendering;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models.Partial
{
    public class PlayerItemsViewModelPartial
    {
        public SelectList AvailablePlayers { get; set; }
        public string SelectedPlayerId { get; set; }
    }
}
