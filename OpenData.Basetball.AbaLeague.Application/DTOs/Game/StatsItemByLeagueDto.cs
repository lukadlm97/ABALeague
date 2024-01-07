using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Game
{
    public record StatsItemByLeagueDto(int TeamId, 
                                        string TeamName, 
                                        decimal? Points = null,
                                        decimal? ShotMade2Pt = null,
                                        decimal? ShotAttempted2Pt = null,
                                        decimal? ShotMade3Pt = null,
                                        decimal? ShotAttempted3Pt = null,
                                        decimal? ShotMade1Pt = null,
                                        decimal? ShotAttempted1Pt = null,
                                        decimal? DefensiveRebounds = null,
                                        decimal? OffensiveRebounds = null,
                                        decimal? TotalRebounds = null,
                                        decimal? Assists = null,
                                        decimal? Steals = null,
                                        decimal? Turnover = null,
                                        decimal? InFavoureOfBlock = null,
                                        decimal? AgainstBlock = null,
                                        decimal? CommittedFoul = null,
                                        decimal? ReceivedFoul = null,
                                        decimal? PointFromPain = null,
                                        decimal? PointFrom2ndChance = null,
                                        decimal? PointFromFastBreak = null,
                                        decimal? PlusMinus = null,
                                        decimal? RankValue = null
                    );
}
