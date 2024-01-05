using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetStandingsByLeagueId
{
    public class GetStandingsByLeagueIdQueryHandler 
        : IQueryHandler<GetStandingsByLeagueIdQuery, Maybe<StandingsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStandingsByLeagueIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<StandingsDto>> 
            Handle(GetStandingsByLeagueIdQuery request, CancellationToken cancellationToken)
        {
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            var teams = await _unitOfWork.TeamRepository.GetAll(cancellationToken);
            var countries = await _unitOfWork.CountryRepository.Get(cancellationToken);
            var matchRounds = await _unitOfWork.CalendarRepository.SearchByLeague(request.LeagueId, cancellationToken);
            var results = await _unitOfWork.ResultRepository.SearchByLeague(request.LeagueId, cancellationToken);
            if(league == null || matchRounds == null  || results == null)
            {
                return Maybe<StandingsDto>.None;
            }

            Dictionary<int, (Team, List<(Domain.Entities.Result, Domain.Entities.RoundMatch)>)> scores = new Dictionary<int, (Team, List<(Domain.Entities.Result, Domain.Entities.RoundMatch)>)>();

            foreach(var item in matchRounds.Where(x=>x.DateTime<DateTime.UtcNow))
            {
                var homeTeam = teams.FirstOrDefault(x => x.Id == item.HomeTeamId);
                var awayTeam = teams.FirstOrDefault(x => x.Id == item.AwayTeamId);
                var result = results.FirstOrDefault(x => x.RoundMatchId == item.Id);
                if(homeTeam == null || awayTeam == null ||result == null)
                {
                    continue;
                }

                if(scores.TryGetValue(homeTeam.Id,
                    out (Team team,
                    List<(Domain.Entities.Result, Domain.Entities.RoundMatch)> roundAndResultMatch) homeTeamValue))
                {
                    homeTeamValue.roundAndResultMatch.Add((result, item));
                }
                else
                {
                    scores.TryAdd(homeTeam.Id, (homeTeam,new List<(Domain.Entities.Result, Domain.Entities.RoundMatch)>()
                    {
                        (result, item)
                    }));
                }

                if (scores.TryGetValue(awayTeam.Id,
                   out (Team team,
                   List<(Domain.Entities.Result, Domain.Entities.RoundMatch)> roundAndResultMatch) awayTeamValue))
                {
                    awayTeamValue.roundAndResultMatch.Add((result, item));
                }
                else
                {
                    scores.TryAdd(awayTeam.Id, (awayTeam, new List<(Domain.Entities.Result, Domain.Entities.RoundMatch)>()
                    {
                        (result, item)
                    }));
                }
            }
            List<StandingsItemDto> list = new List<StandingsItemDto>();

            foreach (var item in scores)
            {
                int playedGames = 0;
                int wonGames = 0;
                int lostGames = 0;
                int playedHomeGames = 0;
                int wonHomeGames = 0;
                int lostHomeGames = 0;
                int playedAwayGames = 0;
                int wonAwayGames = 0;
                int lostAwayGames = 0;
                int scoredPoints = 0;
                int receivedPoints = 0;
                int pointDifference = 0;
                int scoredHomePoints = 0;
                int receivedHomePoints = 0;
                int scoredAwayPoints = 0;
                int receivedAwayPoints = 0;

                if (scores.TryGetValue(item.Key,
                   out (Team team,
                   List<(Domain.Entities.Result, Domain.Entities.RoundMatch)> roundAndResultMatch) value))
                {
                    List<bool> recentForm = new List<bool>();
                    List<bool> homeRecentForm = new List<bool>();
                    List<bool> awayRecentForm = new List<bool>();
                    foreach(var (resultValue, roundMatchValue) in value.roundAndResultMatch)
                    {
                        playedGames++;
                        if(roundMatchValue.HomeTeamId == item.Key)
                        {
                            playedHomeGames++;
                            if(resultValue.HomeTeamPoints > resultValue.AwayTeamPoint)
                            {
                                wonHomeGames++;
                                wonGames++;
                                recentForm.Add(true);
                                homeRecentForm.Add(true);
                            }
                            else
                            {
                                lostHomeGames++;
                                lostGames++;
                                recentForm.Add(false);
                                homeRecentForm.Add(false);
                            }
                            scoredPoints += resultValue.HomeTeamPoints;
                            scoredHomePoints += resultValue.HomeTeamPoints;
                            receivedPoints += resultValue.AwayTeamPoint;
                            receivedHomePoints += resultValue.AwayTeamPoint;
                        }
                        else
                        {
                            playedAwayGames++;
                            if (resultValue.HomeTeamPoints < resultValue.AwayTeamPoint)
                            {
                                wonAwayGames++;
                                wonGames++;
                                recentForm.Add(true);
                                awayRecentForm.Add(true);
                            }
                            else
                            {
                                lostAwayGames++;
                                lostGames++;
                                recentForm.Add(false);
                                awayRecentForm.Add(false);
                            }
                            scoredPoints += resultValue.AwayTeamPoint;
                            scoredAwayPoints += resultValue.AwayTeamPoint;
                            receivedPoints += resultValue.HomeTeamPoints;
                            receivedAwayPoints += resultValue.HomeTeamPoints;
                        }
                    }
                    var country = countries.FirstOrDefault(x => x.Id == value.team.CountryId);
                    if (country == null)
                    {
                        continue;
                    }
                    list.Add(new StandingsItemDto(
                                                    value.team.Id,
                                                    value.team.Name,
                                                    country.Id,
                                                    country.CodeIso,
                                                    playedGames,
                                                    wonGames,
                                                    lostGames,
                                                    playedHomeGames,
                                                    wonHomeGames,
                                                    lostHomeGames, 
                                                    playedAwayGames, 
                                                    wonAwayGames, 
                                                    lostAwayGames, 
                                                    scoredPoints, 
                                                    receivedPoints, 
                                                    scoredPoints - receivedPoints, 
                                                    scoredHomePoints, 
                                                    receivedHomePoints, 
                                                    scoredAwayPoints, 
                                                    receivedAwayPoints, 
                                                    recentForm,
                                                    homeRecentForm,
                                                    awayRecentForm));

                }
                else
                {
                    continue;
                }
            }

            return new StandingsDto(league.Id, 
                                    league.OfficalName,
                                    matchRounds.Select(x => x.Round).Distinct().Count(), 
                                    0, 
                                    list.OrderByDescending(x=>x.WonGames)
                                            .ThenBy(x => x.LostGames)
                                            .ThenByDescending(x=>x.PointDifference));
        }
    }
}
