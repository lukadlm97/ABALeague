namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class TeamIndexViewModel
    {
        public IList<TeamDto> Teams { get; set; }
        public int Number { get; set; }
        public int Size { get; set; }
        public string Filter { get; set; }
    }

    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Country { get; set; }
    }
}
