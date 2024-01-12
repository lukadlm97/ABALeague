using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using OpenData.Basketball.AbaLeague.Application.Services.Implementation;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamRangeStatsByLeagueId
{
    public class GetTeamRangeStatsByLeagueIdQueryHandler :
        IQueryHandler<GetTeamRangeStatsByLeagueIdQuery, Maybe<TeamRangeStatsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGameService _gameService;

        public GetTeamRangeStatsByLeagueIdQueryHandler(IUnitOfWork unitOfWork,
                                                        IGameService gameService)
        {
            _unitOfWork = unitOfWork;
            _gameService = gameService;
        }
        public async Task<Maybe<TeamRangeStatsDto>>
            Handle(GetTeamRangeStatsByLeagueIdQuery request, CancellationToken cancellationToken)
        {
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            if(league == null)
            {
                return Maybe<TeamRangeStatsDto>.None;
            }

            var leagueGameLengths = _unitOfWork.LeagueGameLengthRepository.Get();
            var leagueGameLength = leagueGameLengths.FirstOrDefault(x=>x.LeagueId ==  request.LeagueId);
            if(leagueGameLength == null)
            {
                return Maybe<TeamRangeStatsDto>.None;
            }

            var rangeScalesAvailableByLeague = _unitOfWork.RangeScalesRepository
                                               .Get()
                                               .Where(x=> x.GameLengthId == leagueGameLength.GameLengthId)
                                               .ToList()
                                               .Where(x=>request.StatsProperties.Contains(x.StatsPropertyEnum));
            if(rangeScalesAvailableByLeague == null || !rangeScalesAvailableByLeague.Any())
            {
                return Maybe<TeamRangeStatsDto>.None;
            }

            var teamsThatParticipateAtLeague = await _unitOfWork.SeasonResourcesRepository
                                                                    .SearchByLeague(request.LeagueId, cancellationToken);
            if(teamsThatParticipateAtLeague == null || !teamsThatParticipateAtLeague.Any())
            {
                return Maybe<TeamRangeStatsDto>.None;
            }

            Dictionary<(int, string), FrozenSet<TeamRangeStatsItemDto>> dictionary = 
                new Dictionary<(int, string), FrozenSet<TeamRangeStatsItemDto>>();
            foreach (var teamItem in teamsThatParticipateAtLeague.Select(x => x.Team))
            {
                var results = await _gameService
                    .GetPlayedByLeagueIdAndTeamId(request.LeagueId, teamItem.Id, false, cancellationToken);
                List<TeamRangeStatsItemDto> list = new List<TeamRangeStatsItemDto>();

                foreach (var availabeRangesEnum in rangeScalesAvailableByLeague.Select(x=>x.StatsPropertyEnum).Distinct())
                {
                    Dictionary<int, int> offensiveWinCounter= new Dictionary<int, int>();
                    Dictionary<int, int> offensiveLostCounter= new Dictionary<int, int>();
                    Dictionary<int, int> defensiveWinCounter = new Dictionary<int, int>();
                    Dictionary<int, int> defensiveLostCounter = new Dictionary<int, int>();
                    int other = 0;
                    switch (availabeRangesEnum)
                    {
                        case Domain.Enums.StatsPropertyEnum.Points:
                            var points = results.games.Select(x => (x.Points, x.WinTheGame));
                            foreach (var (pointItem, isWin) in points)
                            {
                                foreach (var availabeRangeItem in rangeScalesAvailableByLeague
                                        .Where(x=>x.StatsPropertyEnum == availabeRangesEnum))
                                {
                                    if(availabeRangeItem.MinValue == null)
                                    {
                                        if(pointItem <= availabeRangeItem.MaxValue)
                                        {
                                            if (isWin)
                                            {
                                                if (!offensiveWinCounter.ContainsKey(availabeRangeItem.Id))
                                                {
                                                    offensiveWinCounter[availabeRangeItem.Id] = 0;
                                                }
                                                offensiveWinCounter[availabeRangeItem.Id]++;
                                            }
                                            else
                                            {
                                                if (!offensiveLostCounter.ContainsKey(availabeRangeItem.Id))
                                                {
                                                    offensiveLostCounter[availabeRangeItem.Id] = 0;
                                                }
                                                offensiveLostCounter[availabeRangeItem.Id]++;
                                            }
                                            break;
                                        }
                                    }
                                    
                                    if (availabeRangeItem.MaxValue == null)
                                    {
                                        if (pointItem >= availabeRangeItem.MinValue)
                                        {
                                            if (isWin)
                                            {
                                                if (!offensiveWinCounter.ContainsKey(availabeRangeItem.Id))
                                                {
                                                    offensiveWinCounter[availabeRangeItem.Id] = 0;
                                                }
                                                offensiveWinCounter[availabeRangeItem.Id]++;
                                            }
                                            else
                                            {
                                                if (!offensiveLostCounter.ContainsKey(availabeRangeItem.Id))
                                                {
                                                    offensiveLostCounter[availabeRangeItem.Id] = 0;
                                                }
                                                offensiveLostCounter[availabeRangeItem.Id]++;
                                            }
                                            break;
                                        }
                                    }
                                    if (availabeRangeItem.MaxValue < pointItem)
                                    {
                                        continue;
                                    }
                                    if (pointItem >= availabeRangeItem.MinValue  && 
                                        pointItem <= availabeRangeItem.MaxValue)
                                    {
                                        if (isWin)
                                        {
                                            if (!offensiveWinCounter.ContainsKey(availabeRangeItem.Id))
                                            {
                                                offensiveWinCounter[availabeRangeItem.Id] = 0;
                                            }
                                            offensiveWinCounter[availabeRangeItem.Id]++;
                                        }
                                        else
                                        {
                                            if (!offensiveLostCounter.ContainsKey(availabeRangeItem.Id))
                                            {
                                                offensiveLostCounter[availabeRangeItem.Id] = 0;
                                            }
                                            offensiveLostCounter[availabeRangeItem.Id]++;
                                        }
                                        break;
                                    }
                                    other++;
                                }
                            }
                            var oponentPoints = results.games.Select(x => (x.OponentPoints, x.WinTheGame));
                            foreach (var (pointItem, isWin) in oponentPoints)
                            {
                                foreach (var availabeRangeItem in rangeScalesAvailableByLeague
                                        .Where(x => x.StatsPropertyEnum == availabeRangesEnum))
                                {
                                    if (availabeRangeItem.MinValue == null)
                                    {
                                        if (pointItem <= availabeRangeItem.MaxValue)
                                        {
                                            if (isWin)
                                            {
                                                if (!defensiveWinCounter.ContainsKey(availabeRangeItem.Id))
                                                {
                                                    defensiveWinCounter[availabeRangeItem.Id] = 0;
                                                }
                                                defensiveWinCounter[availabeRangeItem.Id]++;
                                            }
                                            else
                                            {
                                                if (!defensiveLostCounter.ContainsKey(availabeRangeItem.Id))
                                                {
                                                    defensiveLostCounter[availabeRangeItem.Id] = 0;
                                                }
                                                defensiveLostCounter[availabeRangeItem.Id]++;
                                            }
                                            break;
                                        }
                                    }

                                    if (availabeRangeItem.MaxValue == null)
                                    {
                                        if (pointItem >= availabeRangeItem.MinValue)
                                        {
                                            if (isWin)
                                            {
                                                if (!defensiveWinCounter.ContainsKey(availabeRangeItem.Id))
                                                {
                                                    defensiveWinCounter[availabeRangeItem.Id] = 0;
                                                }
                                                defensiveWinCounter[availabeRangeItem.Id]++;
                                            }
                                            else
                                            {
                                                if (!defensiveLostCounter.ContainsKey(availabeRangeItem.Id))
                                                {
                                                    defensiveLostCounter[availabeRangeItem.Id] = 0;
                                                }
                                                defensiveLostCounter[availabeRangeItem.Id]++;
                                            }
                                            break;
                                        }
                                    }
                                    if (availabeRangeItem.MaxValue < pointItem)
                                    {
                                        continue;
                                    }
                                    if (pointItem >= availabeRangeItem.MinValue &&
                                        pointItem <= availabeRangeItem.MaxValue)
                                    {
                                        if (isWin)
                                        {
                                            if (!defensiveWinCounter.ContainsKey(availabeRangeItem.Id))
                                            {
                                                defensiveWinCounter[availabeRangeItem.Id] = 0;
                                            }
                                            defensiveWinCounter[availabeRangeItem.Id]++;
                                        }
                                        else
                                        {
                                            if (!defensiveLostCounter.ContainsKey(availabeRangeItem.Id))
                                            {
                                                defensiveLostCounter[availabeRangeItem.Id] = 0;
                                            }
                                            defensiveLostCounter[availabeRangeItem.Id]++;
                                        }
                                        break;
                                    }
                                    other++;
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    foreach(var range in rangeScalesAvailableByLeague
                                                            .Where(x => x.StatsPropertyEnum == availabeRangesEnum)
                                                            .OrderBy(x => x.Id)
                                                            .ToList())
                    {
                        int offensiveWinCount = 0;
                        int offensiveLossCount = 0;
                        int defensiveWinCount = 0;
                        int defensiveLossCount = 0;
                        offensiveWinCounter.TryGetValue(range.Id, out offensiveWinCount);
                        offensiveLostCounter.TryGetValue(range.Id, out offensiveLossCount);
                        defensiveWinCounter.TryGetValue(range.Id, out defensiveWinCount);
                        defensiveLostCounter.TryGetValue(range.Id, out defensiveLossCount);
                    
                        list.Add(new TeamRangeStatsItemDto(range.Id, 
                                                            range.MinValue, 
                                                            range.MaxValue, 
                                                            offensiveWinCount,
                                                            offensiveLossCount,
                                                            defensiveWinCount,
                                                            defensiveLossCount,
                                                            range.StatsPropertyEnum));
                          
                    }
                }
                dictionary.Add((teamItem.Id, teamItem.Name), list.ToFrozenSet());
            }

            return new TeamRangeStatsDto(league.Id, league.OfficalName, dictionary.ToFrozenDictionary());

        }
    }
}
