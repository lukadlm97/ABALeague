﻿namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class LeaguePerformanceByPositionItemViewModel 
    {
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public IList<PerformanceByPositionItemViewModel> PerformanceByPositions { get; set; }
    }
}
