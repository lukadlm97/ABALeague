using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Constants
{
    public class CacheKeyConstants
    {
        public const string BoxscoreByRoundAndLeagueId = "BoxscoreByRound{0}ByLeagueId{1}";
        public const string ScheduleByLeagueId = "ScheduleByLeagueId{0}";
        public const string ScoresByLeagueId = "ScoresByLeagueId{0}";
    }
}
