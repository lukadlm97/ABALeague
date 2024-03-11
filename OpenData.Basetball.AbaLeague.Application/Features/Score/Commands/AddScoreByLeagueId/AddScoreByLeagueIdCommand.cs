using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.Constants;
using OpenData.Basketball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.DTOs.Score;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Score.Commands.AddScoreByLeagueId
{
    public class AddScoreByLeagueIdCommand : ICommand<Result>, 
                                                ICacheInvalidatorMediatrQuery<AddScoreByLeagueIdCommand>
    {
        public int LeagueId { get; set; }
        public IEnumerable<AddScoreDto> AddScoreItems { get; set; }

        public bool BypassCache => false;

        public string CacheKey => CacheKeyConstants.ScheduleByLeagueId;

        public AddScoreByLeagueIdCommand(int leagueId, IEnumerable<AddScoreDto> scores)
        {
            LeagueId = leagueId;
            AddScoreItems = scores;
        }
    }
}
