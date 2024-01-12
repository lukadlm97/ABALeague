namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class RangeStatsItemViewModel
    {
        public int Id { get; set; }
         public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public int OffensiveWinCount { get; set; }
        public int OffensiveLossCount { get; set; }
        public int DefensiveWinCount { get; set; }
        public int DefensiveLossCount { get; set; }
        public string StatsName { get; set; }
    }
}
