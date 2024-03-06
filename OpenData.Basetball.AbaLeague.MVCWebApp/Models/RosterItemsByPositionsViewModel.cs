﻿using OpenData.Basetball.AbaLeague.Domain.Enums;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class RosterItemsByPositionsViewModel
    {
        public IList<RosterItemByPositionViewModel> RosterItems { get; set; }
        public IList<(PositionEnum key, string color)> PositionItems { get; set; }
    }
}
