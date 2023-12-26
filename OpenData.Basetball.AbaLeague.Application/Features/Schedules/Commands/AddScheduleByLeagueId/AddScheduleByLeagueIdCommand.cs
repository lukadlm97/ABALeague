using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Schedules.Commands.AddScheduleByLeagueId
{
    public class AddScheduleByLeagueIdCommand : ICommand<Result>
    {
        public IEnumerable<AddScheduleDto> AddScheduleItems { get; init; }
        public int LeagueId { get; init; }
        public bool IsOffSeason { get; init; }

        public AddScheduleByLeagueIdCommand(IEnumerable<AddScheduleDto> addScheduleItems, int leagueId, bool isOffSeason = false)
        {
            AddScheduleItems = addScheduleItems;
            LeagueId = leagueId;
            IsOffSeason = isOffSeason;
        }
    }
}
