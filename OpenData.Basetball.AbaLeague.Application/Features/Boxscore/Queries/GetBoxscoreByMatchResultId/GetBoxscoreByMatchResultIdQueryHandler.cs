using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Queries.GetBoxscoreByMatchResultId
{
    public class GetBoxscoreByMatchResultIdQueryHandler :
        IQueryHandler<GetBoxscoreByMatchResultIdQuery, Maybe<BoxscoreByMatchResultDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBoxscoreByMatchResultIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Maybe<BoxscoreByMatchResultDto>> 
            Handle(GetBoxscoreByMatchResultIdQuery request, 
                    CancellationToken cancellationToken)
        {
            var matchResult = await _unitOfWork.ResultRepository
                                                .GetByMatchResult(request.MatchResultId, cancellationToken);
            if(matchResult == null)
            {
                return Maybe<BoxscoreByMatchResultDto>.None;
            }

            var league = _unitOfWork.LeagueRepository.SearchLeagueByRoundMatchId(matchResult.RoundMatchId);
            if(league == null)
            {
                return Maybe<BoxscoreByMatchResultDto>.None;
            }

            var homeTeamRosterItems = (await _unitOfWork.RosterRepository
                                                        .SearchRosterByLeagueAndTeamId(league.Id, 
                                                        matchResult.RoundMatch.HomeTeamId ?? 0,
                                                        cancellationToken)).ToList();
            var awayTeamRosterItems = (await _unitOfWork.RosterRepository
                                                        .SearchRosterByLeagueAndTeamId(league.Id,
                                                        matchResult.RoundMatch.AwayTeamId ?? 0,
                                                        cancellationToken)).ToList();

            var homeTeamRosterItemIds = homeTeamRosterItems
                                                        .Select(x => x.Id); 
            var awayTeamRosterItemIds = awayTeamRosterItems
                                                        .Select(x => x.Id);

            List<BoxScoreItemDto> homeTeamBoxscores = new List<BoxScoreItemDto>();
            List<BoxScoreItemDto> awayTeamBoxscores = new List<BoxScoreItemDto>();
            var homeBoxscoreItems = _unitOfWork.BoxScoreRepository
                .SearchByMatchRoundAndRosterIds(matchResult.RoundMatchId, homeTeamRosterItemIds)
                .ToList();
            var awayBoxscoreItems = _unitOfWork.BoxScoreRepository
                .SearchByMatchRoundAndRosterIds(matchResult.RoundMatchId, awayTeamRosterItemIds)
                .ToList();
            foreach (var boxscoreItem in homeBoxscoreItems)
            {
                var rosterItem = homeTeamRosterItems.FirstOrDefault(x => x.Id == boxscoreItem.RosterItemId);
                if(rosterItem == null)
                {
                    continue;
                }
                homeTeamBoxscores.Add(new BoxScoreItemDto(boxscoreItem.RosterItemId, 
                                                            matchResult.RoundMatch.Id, 
                                                            rosterItem.Player.Name,
                                                            rosterItem.Player.PositionEnum,
                                                            rosterItem.Team.Name, 
                                                            matchResult.RoundMatch.Round, 
                                                            matchResult.RoundMatch.MatchNo,
                                                            boxscoreItem.Minutes, 
                                                            boxscoreItem.Points,
                                                            boxscoreItem.ShotPrc, 
                                                            boxscoreItem.ShotMade2Pt, 
                                                            boxscoreItem.ShotAttempted2Pt, 
                                                            boxscoreItem.ShotPrc2Pt, 
                                                            boxscoreItem.ShotMade3Pt, 
                                                            boxscoreItem.ShotAttempted3Pt,
                                                            boxscoreItem.shotPrc3Pt, 
                                                            boxscoreItem.ShotMade1Pt, 
                                                            boxscoreItem.ShotAttempted1Pt,
                                                            boxscoreItem.ShotPrc1Pt, 
                                                            boxscoreItem.DefensiveRebounds, 
                                                            boxscoreItem.OffensiveRebounds, 
                                                            boxscoreItem.TotalRebounds, 
                                                            boxscoreItem.Assists,
                                                            boxscoreItem.Steals, 
                                                            boxscoreItem.Turnover, 
                                                            boxscoreItem.InFavoureOfBlock, 
                                                            boxscoreItem.AgainstBlock, 
                                                            boxscoreItem.CommittedFoul, 
                                                            boxscoreItem.ReceivedFoul, 
                                                            boxscoreItem.PointFromPain, 
                                                            boxscoreItem.PointFrom2ndChance, 
                                                            boxscoreItem.PointFromFastBreak, 
                                                            boxscoreItem.PlusMinus, 
                                                            boxscoreItem.RankValue,
                                                            rosterItem.PlayerId));
            }

            foreach (var boxscoreItem in awayBoxscoreItems)
            {
                var rosterItem = awayTeamRosterItems.FirstOrDefault(x => x.Id == boxscoreItem.RosterItemId);
                if (rosterItem == null)
                {
                    continue;
                }
                awayTeamBoxscores.Add(new BoxScoreItemDto(boxscoreItem.RosterItemId,
                                                            matchResult.RoundMatch.Id,
                                                            rosterItem.Player.Name,
                                                            rosterItem.Player.PositionEnum,
                                                            rosterItem.Team.Name,
                                                            matchResult.RoundMatch.Round,
                                                            matchResult.RoundMatch.MatchNo,
                                                            boxscoreItem.Minutes,
                                                            boxscoreItem.Points,
                                                            boxscoreItem.ShotPrc,
                                                            boxscoreItem.ShotMade2Pt,
                                                            boxscoreItem.ShotAttempted2Pt,
                                                            boxscoreItem.ShotPrc2Pt,
                                                            boxscoreItem.ShotMade3Pt,
                                                            boxscoreItem.ShotAttempted3Pt,
                                                            boxscoreItem.shotPrc3Pt,
                                                            boxscoreItem.ShotMade1Pt,
                                                            boxscoreItem.ShotAttempted1Pt,
                                                            boxscoreItem.ShotPrc1Pt,
                                                            boxscoreItem.DefensiveRebounds,
                                                            boxscoreItem.OffensiveRebounds,
                                                            boxscoreItem.TotalRebounds,
                                                            boxscoreItem.Assists,
                                                            boxscoreItem.Steals,
                                                            boxscoreItem.Turnover,
                                                            boxscoreItem.InFavoureOfBlock,
                                                            boxscoreItem.AgainstBlock,
                                                            boxscoreItem.CommittedFoul,
                                                            boxscoreItem.ReceivedFoul,
                                                            boxscoreItem.PointFromPain,
                                                            boxscoreItem.PointFrom2ndChance,
                                                            boxscoreItem.PointFromFastBreak,
                                                            boxscoreItem.PlusMinus,
                                                            boxscoreItem.RankValue,
                                                            rosterItem.PlayerId));
            }


            return new BoxscoreByMatchResultDto(new DTOs.Round.FinishedRoundMatchDto(matchResult.RoundMatch.HomeTeamId ?? 0,
                matchResult.RoundMatch.AwayTeamId ?? 0, 
                matchResult.RoundMatch.HomeTeam.Name,
                matchResult.RoundMatch.AwayTeam.Name, 
                matchResult.HomeTeamPoints, 
                matchResult.AwayTeamPoint, 
                matchResult.RoundMatch.Round,
                matchResult.RoundMatch.MatchNo,
                matchResult.RoundMatch.DateTime,
                matchResult.Attendency,
                matchResult.Venue,
                new BoxscoreTotalDto(null, 
                homeTeamBoxscores.Select(x => x.Points).Sum(),
                null,
                homeTeamBoxscores.Select(x => x.ShotMade2Pt).Sum(),
                homeTeamBoxscores.Select(x => x.ShotAttempted2Pt).Sum(),
                Math.Round((decimal) (homeTeamBoxscores.Select(x => x.ShotMade2Pt).Sum() ?? 0) / homeTeamBoxscores.Select(x => x.ShotAttempted2Pt).Sum() ?? 1,2),
                homeTeamBoxscores.Select(x => x.ShotMade3Pt).Sum(),
                homeTeamBoxscores.Select(x => x.ShotAttempted3Pt).Sum(),
                Math.Round((decimal) (homeTeamBoxscores.Select(x => x.ShotMade3Pt).Sum() ?? 0) / homeTeamBoxscores.Select(x => x.ShotAttempted3Pt).Sum() ?? 1, 2),
                homeTeamBoxscores.Select(x => x.ShotMade1Pt).Sum(),
                homeTeamBoxscores.Select(x => x.ShotAttempted1Pt).Sum(),
                Math.Round((decimal) (homeTeamBoxscores.Select(x => x.ShotMade1Pt).Sum() ?? 0) / homeTeamBoxscores.Select(x => x.ShotAttempted1Pt).Sum() ?? 1, 2),
                homeTeamBoxscores.Select(x=>x.DefensiveRebounds).Sum(),
                homeTeamBoxscores.Select(x=>x.OffensiveRebounds).Sum(),
                homeTeamBoxscores.Select(x=>x.TotalRebounds).Sum(),
                homeTeamBoxscores.Select(x=>x.Assists).Sum(),
                homeTeamBoxscores.Select(x=>x.Steals).Sum(),
                homeTeamBoxscores.Select(x=>x.Turnover).Sum(),
                homeTeamBoxscores.Select(x=>x.InFavoureOfBlock).Sum(),
                homeTeamBoxscores.Select(x=>x.AgainstBlock).Sum(),
                homeTeamBoxscores.Select(x=>x.CommittedFoul).Sum(),
                homeTeamBoxscores.Select(x=>x.ReceivedFoul).Sum(),
                homeTeamBoxscores.Select(x=>x.PointFromPain).Sum(),
                homeTeamBoxscores.Select(x=>x.PointFrom2ndChance).Sum(),
                homeTeamBoxscores.Select(x=>x.PointFromFastBreak).Sum(),
                homeTeamBoxscores.Select(x=>x.PlusMinus).Sum(),
                homeTeamBoxscores.Select(x=>x.RankValue).Sum()
                ), 
                new BoxscoreTotalDto(null,
                awayTeamBoxscores.Select(x => x.Points).Sum(),
                null,
                awayTeamBoxscores.Select(x => x.ShotMade2Pt).Sum(),
                awayTeamBoxscores.Select(x => x.ShotAttempted2Pt).Sum(),
                Math.Round((decimal) (awayTeamBoxscores.Select(x => x.ShotMade2Pt).Sum() ?? 0) / awayTeamBoxscores.Select(x => x.ShotAttempted2Pt).Sum() ?? 1, 2),
                awayTeamBoxscores.Select(x => x.ShotMade3Pt).Sum(),
                awayTeamBoxscores.Select(x => x.ShotAttempted3Pt).Sum(),
                Math.Round((decimal) (awayTeamBoxscores.Select(x => x.ShotMade3Pt).Sum() ?? 0) / awayTeamBoxscores.Select(x => x.ShotAttempted3Pt).Sum() ?? 1, 2),
                awayTeamBoxscores.Select(x => x.ShotMade1Pt).Sum(),
                awayTeamBoxscores.Select(x => x.ShotAttempted1Pt).Sum(),
                Math.Round((decimal) (awayTeamBoxscores.Select(x => x.ShotMade1Pt).Sum() ?? 0) / awayTeamBoxscores.Select(x => x.ShotAttempted1Pt).Sum() ?? 1, 2),
                awayTeamBoxscores.Select(x => x.DefensiveRebounds).Sum(),
                awayTeamBoxscores.Select(x => x.OffensiveRebounds).Sum(),
                awayTeamBoxscores.Select(x => x.TotalRebounds).Sum(),
                awayTeamBoxscores.Select(x => x.Assists).Sum(),
                awayTeamBoxscores.Select(x => x.Steals).Sum(),
                awayTeamBoxscores.Select(x => x.Turnover).Sum(),
                awayTeamBoxscores.Select(x => x.InFavoureOfBlock).Sum(),
                awayTeamBoxscores.Select(x => x.AgainstBlock).Sum(),
                awayTeamBoxscores.Select(x => x.CommittedFoul).Sum(),
                awayTeamBoxscores.Select(x => x.ReceivedFoul).Sum(),
                awayTeamBoxscores.Select(x => x.PointFromPain).Sum(),
                awayTeamBoxscores.Select(x => x.PointFrom2ndChance).Sum(),
                awayTeamBoxscores.Select(x => x.PointFromFastBreak).Sum(),
                awayTeamBoxscores.Select(x => x.PlusMinus).Sum(),
                awayTeamBoxscores.Select(x => x.RankValue).Sum())),
                homeTeamBoxscores.ToFrozenSet(), 
                awayTeamBoxscores.ToFrozenSet());
        }
    }
}
