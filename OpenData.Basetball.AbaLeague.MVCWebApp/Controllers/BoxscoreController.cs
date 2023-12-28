using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Application.DTOs.Score;
using OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Queries.GetBoxscoreDraftByMatchResultId;
using OpenData.Basketball.AbaLeague.Application.Features.Score.Commands.AddScoreByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Score.Queries.GetScoreDraftByLeagueId;
using OpenData.Basketball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Controllers
{
    [Route("[controller]/[action]")]
    public class BoxscoreController : Controller
    {
        private readonly ILogger<BoxscoreController> _logger;
        private readonly ISender _sender;

        public BoxscoreController(ILogger<BoxscoreController> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        [Route("{leagueId:int}/{matchResultId:int}")]
        public async Task<IActionResult> Index(int leagueId,
                                                int matchResultId,
                                                CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetBoxscoreDraftByMatchResultIdQuery(leagueId,matchResultId), cancellationToken);

            if (result.HasNoValue)
            {
                return View("Error");
            }

            ViewBag.Title = "Boxscore Managment";

            var boxscoreViewModel = new BoxscoreViewModel
            {
                DraftBoxscoreItems = result.Value.DraftItems.Select(x => new BoxscoreItemViewModel()
                {
                    RosterItemId = x.RosterItemId,
                    PlayerName= x.PlayerName,
                    MatchNo = x.MatchNo,
                    MatchRoundId = x.MatchRoundId,
                    Round = x.Round,
                    TeamName = x.TeamName,
                    Minutes = x.Minutes,
                    Points = x.Points,
                    ShotPrc = x.ShotPrc,
                    ShotMade2Pt = x.ShotMade2Pt,
                    ShotAttempted2Pt = x.ShotAttempted2Pt,
                    ShotPrc2Pt = x.ShotPrc2Pt,
                    ShotMade3Pt = x.ShotMade3Pt,
                    ShotAttempted3Pt = x.ShotAttempted3Pt,
                    shotPrc3Pt = x.ShotPrc3Pt,
                    ShotMade1Pt = x.ShotMade1Pt,
                    ShotAttempted1Pt = x.ShotAttempted1Pt,
                    ShotPrc1Pt = x.ShotPrc1Pt,
                    DefensiveRebounds = x.DefensiveRebounds,
                    OffensiveRebounds = x.OffensiveRebounds,
                    TotalRebounds = x.TotalRebounds,
                    Assists = x.Assists,
                    Steals = x.Steals,
                    Turnover = x.Turnover,
                    InFavoureOfBlock = x.InFavoureOfBlock,
                    AgainstBlock = x.AgainstBlock,
                    CommittedFoul = x.CommittedFoul,
                    ReceivedFoul = x.ReceivedFoul,
                    PointFromPain = x.PointFromPain,
                    PointFrom2ndChance = x.PointFrom2ndChance,
                    PointFromFastBreak = x.PointFromFastBreak,
                    PlusMinus = x.PlusMinus,
                    RankValue = x.RankValue
                }).ToList(),
                DraftRosterItems = result.Value.DraftRosterItems.Select(x => new DraftRosterItemViewModel()
                {
                    TeamName = x.TeamName,
                    PlayerName = x.PlayerName,
                    TeamId = x.TeamId,
                    LeagueId = x.LeagueId,
                    LeagueName = x.LeagueName,
                    PlayerId = x.PlayerId
                }).ToList(),
                LeagueId = leagueId,
                MatchRoundId = matchResultId,
                ExistingBoxscoreItems = result.Value.ExistingItems.Select(x => new BoxscoreItemViewModel()
                {
                    RosterItemId = x.RosterItemId,
                    PlayerName = x.PlayerName,
                    MatchNo = x.MatchNo,
                    MatchRoundId = x.MatchRoundId,
                    Round = x.Round,
                    TeamName = x.TeamName,
                    Minutes = x.Minutes,
                    Points = x.Points,
                    ShotPrc = x.ShotPrc,
                    ShotMade2Pt = x.ShotMade2Pt,
                    ShotAttempted2Pt = x.ShotAttempted2Pt,
                    ShotPrc2Pt = x.ShotPrc2Pt,
                    ShotMade3Pt = x.ShotMade3Pt,
                    ShotAttempted3Pt = x.ShotAttempted3Pt,
                    shotPrc3Pt = x.ShotPrc3Pt,
                    ShotMade1Pt = x.ShotMade1Pt,
                    ShotAttempted1Pt = x.ShotAttempted1Pt,
                    ShotPrc1Pt = x.ShotPrc1Pt,
                    DefensiveRebounds = x.DefensiveRebounds,
                    OffensiveRebounds = x.OffensiveRebounds,
                    TotalRebounds = x.TotalRebounds,
                    Assists = x.Assists,
                    Steals = x.Steals,
                    Turnover = x.Turnover,
                    InFavoureOfBlock = x.InFavoureOfBlock,
                    AgainstBlock = x.AgainstBlock,
                    CommittedFoul = x.CommittedFoul,
                    ReceivedFoul = x.ReceivedFoul,
                    PointFromPain = x.PointFromPain,
                    PointFrom2ndChance = x.PointFrom2ndChance,
                    PointFromFastBreak = x.PointFromFastBreak,
                    PlusMinus = x.PlusMinus,
                    RankValue = x.RankValue
                }).ToList(),
            };

            return View(boxscoreViewModel);
        }
    }
}
