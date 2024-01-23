using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Score.Commands.AddScoreByLeagueId
{
    public class AddScoreByLeagueIdCommandHandler : ICommandHandler<AddScoreByLeagueIdCommand, Domain.Common.Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddScoreByLeagueIdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Domain.Common.Result> Handle(AddScoreByLeagueIdCommand request, CancellationToken cancellationToken)
        {
            if (request == null || !request.AddScoreItems.Any())
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

                foreach (var item in request.AddScoreItems)
                {
                    if (await _unitOfWork.ResultRepository.Exists(item.MatchId, cancellationToken))
                    {
                        continue;
                    }

                    var homeTeam = await _unitOfWork.TeamRepository.Get(item.HomeTeamId, cancellationToken);
                    var awayTeam = await _unitOfWork.TeamRepository.Get(item.AwayTeamId, cancellationToken);
                    if (homeTeam == null || awayTeam == null)
                    {
                        continue;
                    }
                    var calendarItem =
                         await _unitOfWork.CalendarRepository.Get(item.MatchId, cancellationToken);
                    var newItem = new Domain.Entities.Result()
                    {
                        RoundMatch = calendarItem,
                        RoundMatchId = calendarItem.Id,
                        Attendency = item.Attendency,
                        Venue = item.Venue ?? string.Empty,
                        AwayTeamPoint = item.AwayTeamPoints,
                        HomeTeamPoints = item.HomeTeamPoints
                    };
                    await _unitOfWork.ResultRepository.Add(newItem, cancellationToken);
                }

                await _unitOfWork.Save(cancellationToken);
            }
            catch (Exception ex)
            {
                return Domain.Common.Result.Failure(new Error("000", "Problem on saving new items"));
            }


            return Domain.Common.Result.Success();
        }
    }
}
