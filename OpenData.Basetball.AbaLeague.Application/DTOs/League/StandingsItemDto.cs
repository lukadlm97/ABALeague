using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.League
{
    public record StandingsItemDto( 
                                    int TeamId, 
                                    string TeamName,
                                    int CountryId, 
                                    string CountryCode, 
                                    int PlayedGames,
                                    int WonGames, 
                                    int LostGames,
                                    int PlayedHomeGames, 
                                    int WonHomeGames,
                                    int LostHomeGames,
                                    int PlayedAwayGames,
                                    int WonAwayGames,
                                    int LostAwayGames,
                                    int ScoredPoints,
                                    int ReceivedPoints,
                                    int PointDifference,
                                    int ScoredHomePoints,
                                    int ReceivedHomePoints,
                                    int ScoredAwayPoints,
                                    int ReceivedAwayPoints,
                                    IEnumerable<bool> RecentForm,
                                    IEnumerable<bool> HomeRecentForm,
                                    IEnumerable<bool> AwayRecentForm
                                    );
}
