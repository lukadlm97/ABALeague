using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Round;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Result = OpenData.Basketball.AbaLeague.Domain.Common.Result;

namespace OpenData.Basketball.AbaLeague.Application.Features.Schedules.Commands.AddScheduleByLeagueId
{
    public class AddScheduleByLeagueIdCommandHandler : ICommandHandler<AddScheduleByLeagueIdCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddScheduleByLeagueIdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddScheduleByLeagueIdCommand request, CancellationToken cancellationToken)
        {
            if (request == null || !request.AddScheduleItems.Any())
            {
                return Domain.Common.Result.Failure(new Error("000", "not found any new items for insertion"));
            }
            try
            {
                var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
                if (league == null)
                {
                    return Domain.Common.Result.Failure(new Error("000", "not found league "));
                }

                var existingScheduleItems = await _unitOfWork.CalendarRepository.SearchByLeague(request.LeagueId, cancellationToken);

                foreach (var item in request.AddScheduleItems)
                {
                    if (existingScheduleItems.Any(x => x.HomeTeamId == item.HomeTeamId && x.AwayTeamId == item.AwayTeamId))
                    {
                        continue;
                    }

                    var homeTeam = await _unitOfWork.TeamRepository.Get(item.HomeTeamId, cancellationToken);
                    var awayTeam = await _unitOfWork.TeamRepository.Get(item.AwayTeamId, cancellationToken);
                    if (homeTeam == null || awayTeam == null)
                    {
                        continue;

                    }
                    if (league.RoundMatches == null)
                    {
                        league.RoundMatches = new List<RoundMatch>();
                    }

                    if (!request.IsOffSeason)
                    {
                        if (await _unitOfWork.CalendarRepository.Exist(request.LeagueId,
                                item.HomeTeamId,
                                item.AwayTeamId,
                                cancellationToken))
                        {
                            continue;
                        }
                    }

                    var newRoundItem = new RoundMatch()
                    {
                        AwayTeam = awayTeam,
                        AwayTeamId = awayTeam.Id,
                        HomeTeam = homeTeam,
                        HomeTeamId = homeTeam.Id,
                        MatchNo = item.MatchNo,
                        Round = item.Round,
                        OffSeason = request.IsOffSeason,
                        DateTime = item.
                            DateTime.ToUniversalTime(),
                    };

                    league.RoundMatches.Add(newRoundItem);

                    await _unitOfWork.LeagueRepository.Update(league, cancellationToken);
                }
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error("000", "Problem on saving new items"));
            }


            return Result.Success();
        }
    }
}
