using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Commands.AddBoxscoreByMatchResultId
{
    public class AddBoxscoreByMatchResultIdCommand : ICommand<Result>
    {
        public AddBoxscoreByMatchResultIdCommand(int leagueId, IEnumerable<AddBoxScoreDto> boxscores)
        {
            LeagueId = leagueId;
            Boxscores = boxscores;
        }

        public int LeagueId { get; private set; }
        public IEnumerable<AddBoxScoreDto> Boxscores { get; private set; }
    }
}
