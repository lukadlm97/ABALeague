using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Commands.AddBoxscoreByMatchResultId
{
    public class AddBoxscoreByMatchResultIdCommandHandler :
        ICommandHandler<AddBoxscoreByMatchResultIdCommand, Domain.Common.Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddBoxscoreByMatchResultIdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async  Task<Domain.Common.Result> Handle(AddBoxscoreByMatchResultIdCommand request, CancellationToken cancellationToken)
        {
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            if(league == null ||  !request.Boxscores.Any())
            {
                return 
                    Domain.Common.Result.Failure(new Error("000","Unable to find new league, match result or boxscore existing"));
            }

            try
            {
                foreach (var singleBoxscore in request.Boxscores)
                {
                    var rosterItem = await _unitOfWork.RosterRepository
                                                        .Get(singleBoxscore.RosterItemId, cancellationToken);
                    var matchRound = await _unitOfWork.CalendarRepository
                                                        .Get(singleBoxscore.MatchRoundId, cancellationToken);
                    if(rosterItem == null || matchRound == null)
                    {
                        continue;
                    }

                    if (await _unitOfWork.BoxScoreRepository.Exist(matchRound.Id, rosterItem.Id, cancellationToken))
                    {
                        continue;
                    }

                    var newBoxscore = new BoxScore
                    {
                        AgainstBlock = singleBoxscore.AgainstBlock,
                        Assists = singleBoxscore.Assists,   
                        CommittedFoul = singleBoxscore.CommittedFoul,
                        DefensiveRebounds = singleBoxscore.DefensiveRebounds,
                        InFavoureOfBlock = singleBoxscore.InFavoureOfBlock,
                        Minutes = singleBoxscore.Minutes,
                        OffensiveRebounds = singleBoxscore.OffensiveRebounds,
                        PlusMinus = singleBoxscore.PlusMinus,
                        PointFrom2ndChance = singleBoxscore.PointFrom2ndChance,
                        PointFromFastBreak = singleBoxscore.PointFromFastBreak,
                        PointFromPain = singleBoxscore.PointFromPain,
                        Points = singleBoxscore.Points,
                        RankValue = singleBoxscore.RankValue,
                        ReceivedFoul = singleBoxscore.ReceivedFoul,
                        RosterItemId = singleBoxscore.RosterItemId,
                        RoundMatchId = singleBoxscore.MatchRoundId,
                        ShotAttempted1Pt = singleBoxscore.ShotAttempted1Pt,
                        ShotAttempted2Pt = singleBoxscore.ShotAttempted2Pt,
                        ShotAttempted3Pt = singleBoxscore.ShotAttempted3Pt,
                        ShotMade1Pt = singleBoxscore.ShotMade1Pt,
                        ShotMade2Pt = singleBoxscore.ShotMade2Pt,
                        ShotMade3Pt = singleBoxscore.ShotMade3Pt,
                        ShotPrc = singleBoxscore.ShotPrc,
                        ShotPrc1Pt = singleBoxscore.ShotPrc1Pt,
                        ShotPrc2Pt = singleBoxscore.ShotPrc2Pt,
                        shotPrc3Pt = singleBoxscore.ShotPrc3Pt,
                        Steals = singleBoxscore.Steals,
                        TotalRebounds = singleBoxscore.TotalRebounds,
                        Turnover = singleBoxscore.Turnover  
                    };

                    await _unitOfWork.BoxScoreRepository.Add(newBoxscore, cancellationToken);
                }
                await _unitOfWork.Save();


                return Domain.Common.Result.Success();
            }
            catch(Exception ex)
            {
                return Domain.Common.Result.Failure(new Error("100", "Unable to save boxscore at existing infrastructure"));
            }



        }
    }
}
