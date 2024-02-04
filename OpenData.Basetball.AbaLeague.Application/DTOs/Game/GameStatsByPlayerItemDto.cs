﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Game
{
    public record GameStatsByPlayerItemDto(int RoundMatchId,
                            int OponentId,
                            string OponentName,
                            DateTime Date,
                            int Round,
                            int MatchNo,
                            bool HomeGame,
                            bool WinTheGame,
                            string Venue,
                            int Attendency,
                            TimeSpan? Minutes = null,
                            int? Points = null,
                            decimal? ShotPrc = null,
                            int? ShotMade2Pt = null,
                            int? ShotAttempted2Pt = null,
                            decimal? ShotPrc2Pt = null,
                            int? ShotMade3Pt = null,
                            int? ShotAttempted3Pt = null,
                            decimal? ShotPrc3Pt = null,
                            int? ShotMade1Pt = null,
                            int? ShotAttempted1Pt = null,
                            decimal? ShotPrc1Pt = null,
                            int? DefensiveRebounds = null,
                            int? OffensiveRebounds = null,
                            int? TotalRebounds = null,
                            int? Assists = null,
                            int? Steals = null,
                            int? Turnover = null,
                            int? InFavoureOfBlock = null,
                            int? AgainstBlock = null,
                            int? CommittedFoul = null,
                            int? ReceivedFoul = null,
                            int? PointFromPain = null,
                            int? PointFrom2ndChance = null,
                            int? PointFromFastBreak = null,
                            int? PlusMinus = null,
                            int? RankValue = null,
                            string? Result = null,
                            int? MatchResultId = null) : MatchItemDto(RoundMatchId,
                                                                    OponentId,
                                                                    OponentName,
                                                                    -1,
                                                                    new List<bool>(),
                                                                    Date,
                                                                    Round,
                                                                    MatchNo,
                                                                    HomeGame);
}
