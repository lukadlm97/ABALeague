using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basetball.AbaLeague.MVCWebApp.Utilities;
using OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Queries.GetBoxscoreByMatchResultId;
using OpenData.Basketball.AbaLeague.Application.Features.Countries.Queries.GetCountries;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayer;
using OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterHistoryByPlayer;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<HomeController> _logger; 
        private readonly ISender _sender;

        public GameController(ILogger<HomeController> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int matchResultId, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetBoxscoreByMatchResultIdQuery(matchResultId), cancellationToken);

            if (result.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel { Description = "Unable to fetch it"});
            }

            ViewBag.Title = "Game - Details";
            List<PlayerGameStatsViewModel> list = new List<PlayerGameStatsViewModel>();


            return View(new GameDetailsViewModel
            {
                HomaTeamPlayers = result.Value.HomeTeamItems.Select(item => new PlayerGameStatsViewModel
                {
                    RosterItemId = item.RosterItemId,
                    PlayerName = item.PlayerName,
                    RankValue = item.RankValue,
                    AgainstBlock = item.AgainstBlock,
                    Assists = item.Assists,
                    CommittedFoul = item.CommittedFoul,
                    DefensiveRebounds = item.DefensiveRebounds,
                    InFavoureOfBlock = item.InFavoureOfBlock,
                    Minutes = item.Minutes,
                    OffensiveRebounds = item.OffensiveRebounds,
                    PlusMinus = item.PlusMinus,
                    PointFrom2ndChance = item.PointFrom2ndChance,
                    PointFromFastBreak = item.PointFromFastBreak,
                    PointFromPain = item.PointFromPain,
                    Points = item.Points,
                    ReceivedFoul = item.ReceivedFoul,
                    ShotAttempted1Pt = item.ShotAttempted1Pt,
                    ShotAttempted2Pt = item.ShotAttempted2Pt,
                    ShotAttempted3Pt = item.ShotAttempted3Pt,
                    ShotMade1Pt = item.ShotMade1Pt,
                    ShotMade2Pt = item.ShotMade2Pt,
                    ShotMade3Pt = item.ShotMade3Pt,
                    ShotPrc = item.ShotPrc,
                    ShotPrc1Pt = item.ShotPrc1Pt,
                    ShotPrc2Pt = item.ShotPrc2Pt,
                    ShotPrc3Pt = item.ShotPrc3Pt,
                    Steals = item.Steals,
                    TotalRebounds = item.TotalRebounds,
                    Turnover = item.Turnover,
                    PlayerId = item.PlayerId ?? 0,
                    PositionColor = item.PlayerPosition.ConvertPositionEnumToColor()
                }
                ).OrderByDescending(x => x.Points).ThenByDescending(x => x.Assists).ThenByDescending(x => x.TotalRebounds).ToList(),
                AwayTeamPlayers = result.Value.AwayTeamItems.Select(item => new PlayerGameStatsViewModel
                {
                    RosterItemId = item.RosterItemId,
                    PlayerName = item.PlayerName,
                    RankValue = item.RankValue,
                    AgainstBlock = item.AgainstBlock,
                    Assists = item.Assists,
                    CommittedFoul = item.CommittedFoul,
                    DefensiveRebounds = item.DefensiveRebounds,
                    InFavoureOfBlock = item.InFavoureOfBlock,
                    Minutes = item.Minutes,
                    OffensiveRebounds = item.OffensiveRebounds,
                    PlusMinus = item.PlusMinus,
                    PointFrom2ndChance = item.PointFrom2ndChance,
                    PointFromFastBreak = item.PointFromFastBreak,
                    PointFromPain = item.PointFromPain,
                    Points = item.Points,
                    ReceivedFoul = item.ReceivedFoul,
                    ShotAttempted1Pt = item.ShotAttempted1Pt,
                    ShotAttempted2Pt = item.ShotAttempted2Pt,
                    ShotAttempted3Pt = item.ShotAttempted3Pt,
                    ShotMade1Pt = item.ShotMade1Pt,
                    ShotMade2Pt = item.ShotMade2Pt,
                    ShotMade3Pt = item.ShotMade3Pt,
                    ShotPrc = item.ShotPrc,
                    ShotPrc1Pt = item.ShotPrc1Pt,
                    ShotPrc2Pt = item.ShotPrc2Pt,
                    ShotPrc3Pt = item.ShotPrc3Pt,
                    Steals = item.Steals,
                    TotalRebounds = item.TotalRebounds,
                    Turnover = item.Turnover,
                    PlayerId = item.PlayerId ?? 0,
                    PositionColor = item.PlayerPosition.ConvertPositionEnumToColor()
                }).OrderByDescending(x => x.Points).ThenByDescending(x => x.Assists).ThenByDescending(x => x.TotalRebounds).ToList(),
                MatchResult = new MatchResultViewModel
                {
                    Round = result.Value.RoundMatchDetails.Round,
                    Attendency = result.Value.RoundMatchDetails.Attendency ?? 0,
                    Venue = result.Value.RoundMatchDetails.Venue,
                    AwayTeamName = result.Value.RoundMatchDetails.AwayTeamName,
                    HomeTeamName = result.Value.RoundMatchDetails.HomeTeamName,
                    MatchNo = result.Value.RoundMatchDetails.MatchNo,
                    AwayTeamPoints = result.Value.RoundMatchDetails.AwayTeamPoints ?? 0,
                    HomeTeamPoints = result.Value.RoundMatchDetails.HomeTeamPoints ?? 0,
                },
                HomeTeamTotal = new GameStatsViewModel
                {
                    RankValue = result.Value.RoundMatchDetails.HomeTeamTotals.RankValue,
                    AgainstBlock = result.Value.RoundMatchDetails.HomeTeamTotals.AgainstBlock,
                    Assists = result.Value.RoundMatchDetails.HomeTeamTotals.Assists,
                    CommittedFoul = result.Value.RoundMatchDetails.HomeTeamTotals.CommittedFoul,
                    DefensiveRebounds = result.Value.RoundMatchDetails.HomeTeamTotals.DefensiveRebounds,
                    InFavoureOfBlock = result.Value.RoundMatchDetails.HomeTeamTotals.InFavoureOfBlock,
                    Minutes = result.Value.RoundMatchDetails.HomeTeamTotals.Minutes,
                    OffensiveRebounds = result.Value.RoundMatchDetails.HomeTeamTotals.OffensiveRebounds,
                    PlusMinus = result.Value.RoundMatchDetails.HomeTeamTotals.PlusMinus,
                    PointFrom2ndChance = result.Value.RoundMatchDetails.HomeTeamTotals.PointFrom2ndChance,
                    PointFromFastBreak = result.Value.RoundMatchDetails.HomeTeamTotals.PointFromFastBreak,
                    PointFromPain = result.Value.RoundMatchDetails.HomeTeamTotals.PointFromPain,
                    Points = result.Value.RoundMatchDetails.HomeTeamTotals.Points,
                    ReceivedFoul = result.Value.RoundMatchDetails.HomeTeamTotals.ReceivedFoul,
                    ShotAttempted1Pt = result.Value.RoundMatchDetails.HomeTeamTotals.ShotAttempted1Pt,
                    ShotAttempted2Pt = result.Value.RoundMatchDetails.HomeTeamTotals.ShotAttempted2Pt,
                    ShotAttempted3Pt = result.Value.RoundMatchDetails.HomeTeamTotals.ShotAttempted3Pt,
                    ShotMade1Pt = result.Value.RoundMatchDetails.HomeTeamTotals.ShotMade1Pt,
                    ShotMade2Pt = result.Value.RoundMatchDetails.HomeTeamTotals.ShotMade2Pt,
                    ShotMade3Pt = result.Value.RoundMatchDetails.HomeTeamTotals.ShotMade3Pt,
                    ShotPrc = result.Value.RoundMatchDetails.HomeTeamTotals.ShotPrc,
                    ShotPrc1Pt = result.Value.RoundMatchDetails.HomeTeamTotals.ShotPrc1Pt,
                    ShotPrc2Pt = result.Value.RoundMatchDetails.HomeTeamTotals.ShotPrc2Pt,
                    ShotPrc3Pt = result.Value.RoundMatchDetails.HomeTeamTotals.ShotPrc3Pt,
                    Steals = result.Value.RoundMatchDetails.HomeTeamTotals.Steals,
                    TotalRebounds = result.Value.RoundMatchDetails.HomeTeamTotals.TotalRebounds,
                    Turnover = result.Value.RoundMatchDetails.HomeTeamTotals.Turnover
                },
                AwayTeamTotal = new GameStatsViewModel
                {
                    RankValue = result.Value.RoundMatchDetails.AwayTeamTotals.RankValue,
                    AgainstBlock = result.Value.RoundMatchDetails.AwayTeamTotals.AgainstBlock,
                    Assists = result.Value.RoundMatchDetails.AwayTeamTotals.Assists,
                    CommittedFoul = result.Value.RoundMatchDetails.AwayTeamTotals.CommittedFoul,
                    DefensiveRebounds = result.Value.RoundMatchDetails.AwayTeamTotals.DefensiveRebounds,
                    InFavoureOfBlock = result.Value.RoundMatchDetails.AwayTeamTotals.InFavoureOfBlock,
                    Minutes = result.Value.RoundMatchDetails.AwayTeamTotals.Minutes,
                    OffensiveRebounds = result.Value.RoundMatchDetails.AwayTeamTotals.OffensiveRebounds,
                    PlusMinus = result.Value.RoundMatchDetails.AwayTeamTotals.PlusMinus,
                    PointFrom2ndChance = result.Value.RoundMatchDetails.AwayTeamTotals.PointFrom2ndChance,
                    PointFromFastBreak = result.Value.RoundMatchDetails.AwayTeamTotals.PointFromFastBreak,
                    PointFromPain = result.Value.RoundMatchDetails.AwayTeamTotals.PointFromPain,
                    Points = result.Value.RoundMatchDetails.AwayTeamTotals.Points,
                    ReceivedFoul = result.Value.RoundMatchDetails.AwayTeamTotals.ReceivedFoul,
                    ShotAttempted1Pt = result.Value.RoundMatchDetails.AwayTeamTotals.ShotAttempted1Pt,
                    ShotAttempted2Pt = result.Value.RoundMatchDetails.AwayTeamTotals.ShotAttempted2Pt,
                    ShotAttempted3Pt = result.Value.RoundMatchDetails.AwayTeamTotals.ShotAttempted3Pt,
                    ShotMade1Pt = result.Value.RoundMatchDetails.AwayTeamTotals.ShotMade1Pt,
                    ShotMade2Pt = result.Value.RoundMatchDetails.AwayTeamTotals.ShotMade2Pt,
                    ShotMade3Pt = result.Value.RoundMatchDetails.AwayTeamTotals.ShotMade3Pt,
                    ShotPrc = result.Value.RoundMatchDetails.AwayTeamTotals.ShotPrc,
                    ShotPrc1Pt = result.Value.RoundMatchDetails.AwayTeamTotals.ShotPrc1Pt,
                    ShotPrc2Pt = result.Value.RoundMatchDetails.AwayTeamTotals.ShotPrc2Pt,
                    ShotPrc3Pt = result.Value.RoundMatchDetails.AwayTeamTotals.ShotPrc3Pt,
                    Steals = result.Value.RoundMatchDetails.AwayTeamTotals.Steals,
                    TotalRebounds = result.Value.RoundMatchDetails.AwayTeamTotals.TotalRebounds,
                    Turnover = result.Value.RoundMatchDetails.AwayTeamTotals.Turnover
                },
            }) ;
        }
    }
}
