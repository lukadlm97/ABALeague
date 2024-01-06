using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Game
{
    public record MatchItemDto(int MatchRoundId,
                            int OponentId,
                            string OponentName,
                            int OponentCurrentRanking,
                            IEnumerable<bool> OponentRecentForm,
                            DateTime Date,
                            int Round,
                            int MatchNo,
                            bool HomeGame);
}
