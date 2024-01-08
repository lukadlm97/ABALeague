namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class RangeStatsItemViewModel
    {
        public int Id { get; set; }
         public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public int Count { get; set; }
        public string StatsName { get; set; }
    }
}
