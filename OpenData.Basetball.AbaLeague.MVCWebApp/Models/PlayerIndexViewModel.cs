namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class PlayerIndexViewModel
    {
        public IList<PlayerViewModel> Players { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Query { get; set; }
    }
}
