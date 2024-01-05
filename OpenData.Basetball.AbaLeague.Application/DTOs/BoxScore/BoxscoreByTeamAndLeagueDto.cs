﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore
{
    public record BoxscoreByTeamAndLeagueDto(int TeamId,
                                                string TeamName,
                                                int LeagueId,
                                                string LeagueName,
                                                IEnumerable<GameStats> Games,
                                                AverageBoxscoreCalcuationDto AverageBoxscoreCalcuations,
                                                AdvancedMatchCalcuationDto AdvancedMatchCalcuations);
}
