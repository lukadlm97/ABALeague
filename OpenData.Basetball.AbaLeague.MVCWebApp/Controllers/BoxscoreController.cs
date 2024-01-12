using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Application.DTOs.Score;
using OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Commands.AddBoxscoreByMatchResultId;
using OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Commands.AddBoxscoreByRoundNo;
using OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Queries.GetBoxscoreDraftByMatchResultId;
using OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Queries.GetBoxscoreDraftByRound;
using OpenData.Basketball.AbaLeague.Application.Features.Rounds.Queries.GetRoundsByLeagueId;
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

        public async Task<IActionResult> Index(int leagueId, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetRoundsByLeagueIdQuery(leagueId), cancellationToken);
            if (result.HasNoValue)
            {
                return View("Error");
            }
            var indexViewModel = new BoxscoreIndexViewModel 
            { 
                LeagueId = leagueId, 
                AvailableRounds = result.Value.RoundNumbers.ToList() 
            };
            return View(indexViewModel);
        }

        [Route("{leagueId:int}/{matchResultId:int}")]
        public async Task<IActionResult> DraftByMatch(int leagueId,
                                                int matchResultId,
                                                CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetBoxscoreDraftByMatchResultIdQuery(leagueId,matchResultId), cancellationToken);

            if (result.HasNoValue)
            {
                return View("Error");
            }

            ViewBag.Title = "Boxscore Managment";
            var realValuePointsByHomeTeam = result.Value.ExistingItems
                .Where(x => x.TeamName == result.Value.MatchScore.HomeTeamName)
                .Select(x => x.Points).Sum() + result.Value.DraftItems
                .Where(x => x.TeamName == result.Value.MatchScore.HomeTeamName)
                .Select(x => x.Points).Sum() ?? 0;
            var realValuepointsByAwayTeam = result.Value.ExistingItems
                .Where(x => x.TeamName == result.Value.MatchScore.AwayTeamName)
                .Select(x => x.Points).Sum() + result.Value.DraftItems
                .Where(x => x.TeamName == result.Value.MatchScore.AwayTeamName)
                .Select(x => x.Points).Sum() ?? 0;

            var boxscoreViewModel = new BoxscoreDraftByMatchViewModel
            {
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
                MatchResultId = matchResultId,
                HomeDraftBoxscoreItems = result.Value.DraftItems
                .Where(x => x.TeamName == result.Value.MatchScore.HomeTeamName)
                .Select(x => new BoxscoreItemViewModel()
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
                }
                ).ToList(),
                AwayDraftBoxscoreItems = result.Value.DraftItems
                .Where(x => x.TeamName == result.Value.MatchScore.AwayTeamName)
                .Select(x => new BoxscoreItemViewModel()
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
                }
                ).ToList(),
                HomeExistingBoxscoreItems = result.Value.ExistingItems
                .Where(x=>x.TeamName==result.Value.MatchScore.HomeTeamName)
                .Select(x => new BoxscoreItemViewModel()
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
                }
                ).ToList(),
                AwayExistingBoxscoreItems = result.Value.ExistingItems
                .Where(x => x.TeamName == result.Value.MatchScore.AwayTeamName)
                .Select(x => new BoxscoreItemViewModel()
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
                }
                ).ToList(),
                MissingPlayerItems = result.Value.MissingPlayers.ToList(),
                MatchResult = new MatchResultViewModel
                {
                    MatchNo = result.Value.MatchScore.MatchNo,
                    Round = result.Value.MatchScore.Round,
                    Attendency = result.Value.MatchScore.Attendency??0,
                    AwayTeamName = result.Value.MatchScore.AwayTeamName,
                    AwayTeamPoints = result.Value.MatchScore.AwayTeamPoints??0,
                    HomeTeamName = result.Value.MatchScore.HomeTeamName,
                    HomeTeamPoints = result.Value.MatchScore.HomeTeamPoints ?? 0,
                    Venue = result.Value.MatchScore.Venue
                },
                AwayTeamPoints = realValuepointsByAwayTeam,
                HomeTeamPoints = realValuePointsByHomeTeam,
                AwayTeamMatch = realValuepointsByAwayTeam == result.Value.MatchScore.AwayTeamPoints,
                HomeTeamMatch = realValuePointsByHomeTeam == result.Value.MatchScore.HomeTeamPoints,
                AvailableDraftBoxscoreItems = result.Value.DraftItems.Any(),
                AvailableDraftRosterItems = result.Value.DraftRosterItems.Any(),
                AvailableMissingPlayerItems = result.Value.MissingPlayers.Any(),
            };

            return View(boxscoreViewModel);
        }

        [HttpPost]
        [Route("{leagueId:int}/{matchResultId:int}")]
        public async Task<IActionResult> AddByMatch(int leagueId,
                                                int matchResultId,
                                                CancellationToken cancellationToken = default)
        {
            var result = await _sender
                .Send(new GetBoxscoreDraftByMatchResultIdQuery(leagueId, matchResultId), cancellationToken);

            if (result.HasNoValue)
            {
                return View("Error");
            }

            List<AddBoxScoreDto> list = new List<AddBoxScoreDto>();
            foreach (var item in result.Value.DraftItems)
            {
                list.Add(
                    new AddBoxScoreDto(
                        item.RosterItemId,
                        item.MatchRoundId, 
                        item.Round,
                        item.Minutes,
                        item.Points,
                        item.ShotPrc,
                        item.ShotMade2Pt,
                        item.ShotAttempted2Pt,
                        item.ShotPrc2Pt,
                        item.ShotMade3Pt,
                        item.ShotAttempted3Pt,
                        item.ShotPrc3Pt,
                        item.ShotMade1Pt,
                        item.ShotAttempted1Pt,
                        item.ShotPrc1Pt,
                        item.DefensiveRebounds,
                        item.OffensiveRebounds,
                        item.TotalRebounds,
                        item.Assists,
                        item.Steals, 
                        item.Turnover, 
                        item.InFavoureOfBlock,
                        item.AgainstBlock,
                        item.CommittedFoul, 
                        item.ReceivedFoul, 
                        item.PointFromPain,
                        item.PointFrom2ndChance,
                        item.PointFromFastBreak,
                        item.PlusMinus, 
                        item.RankValue)) ;
            }

            var addBoxscoreResult = await _sender
                .Send(new AddBoxscoreByMatchResultIdCommand(leagueId, list), cancellationToken);
           
            if (addBoxscoreResult.IsFailure)
            {
                return View("Error");
            }

            var roundsByLeagueResult = await _sender.Send(new GetRoundsByLeagueIdQuery(leagueId), cancellationToken);
            if (result.HasNoValue)
            {
                return View("Error");
            }
            var indexViewModel = new BoxscoreIndexViewModel
            {
                LeagueId = leagueId,
                AvailableRounds = roundsByLeagueResult.Value.RoundNumbers.ToList()
            };
            return View("Index", indexViewModel);
        }

        [HttpPost]
        [Route("{leagueId:int}/{roundNo:int}")]
        public async Task<IActionResult> AddByRound(int leagueId,
                                               int roundNo,
                                               CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetBoxscoreDraftByRoundQuery(leagueId, roundNo), cancellationToken);

            if (result.HasNoValue)
            {
                return View("Error");
            }

            List<AddBoxScoreDto> list = new List<AddBoxScoreDto>();
            foreach (var item in result.Value.BoxScoreItems.SelectMany(x=>x.DraftItems))
            {
                list.Add(
                    new AddBoxScoreDto(
                        item.RosterItemId,
                        item.MatchRoundId,
                        item.Round,
                        item.Minutes,
                        item.Points,
                        item.ShotPrc,
                        item.ShotMade2Pt,
                        item.ShotAttempted2Pt,
                        item.ShotPrc2Pt,
                        item.ShotMade3Pt,
                        item.ShotAttempted3Pt,
                        item.ShotPrc3Pt,
                        item.ShotMade1Pt,
                        item.ShotAttempted1Pt,
                        item.ShotPrc1Pt,
                        item.DefensiveRebounds,
                        item.OffensiveRebounds,
                        item.TotalRebounds,
                        item.Assists,
                        item.Steals,
                        item.Turnover,
                        item.InFavoureOfBlock,
                        item.AgainstBlock,
                        item.CommittedFoul,
                        item.ReceivedFoul,
                        item.PointFromPain,
                        item.PointFrom2ndChance,
                        item.PointFromFastBreak,
                        item.PlusMinus,
                        item.RankValue));
            }
            var roundNos = result.Value.BoxScoreItems.SelectMany(x => x.DraftItems).Select(x => x.Round).Distinct();
            var addBoxscoreResult = await _sender
                .Send(new AddBoxscoreByRoundNoCommand(leagueId, list, roundNos.First()), cancellationToken);

            if (addBoxscoreResult.IsFailure)
            {
                return View("Error");
            }

            var roundsByLeagueResult = await _sender.Send(new GetRoundsByLeagueIdQuery(leagueId), cancellationToken);
            if (result.HasNoValue)
            {
                return View("Error");
            }
            var indexViewModel = new BoxscoreIndexViewModel
            {
                LeagueId = leagueId,
                AvailableRounds = roundsByLeagueResult.Value.RoundNumbers.ToList()
            };
            return View("Index", indexViewModel);
        }

        [Route("{leagueId:int}/{roundNo:int}")]
        public async Task<IActionResult> DraftByRound(int leagueId,
                                                int roundNo,
                                                CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetBoxscoreDraftByRoundQuery(leagueId, roundNo), cancellationToken);

            if (result.HasNoValue)
            {
                return View("Error");
            }

          
            List<BoxscoreDraftByMatchViewModel> list = new List<BoxscoreDraftByMatchViewModel>();

            foreach(var item in result.Value.BoxScoreItems)
            {
                var realValuePointsByHomeTeam = item.ExistingItems
               .Where(x => x.TeamName == item.MatchScore.HomeTeamName)
               .Select(x => x.Points).Sum() + item.DraftItems
               .Where(x => x.TeamName == item.MatchScore.HomeTeamName)
               .Select(x => x.Points).Sum() ?? 0;
                var realValuepointsByAwayTeam = item.ExistingItems
                    .Where(x => x.TeamName == item.MatchScore.AwayTeamName)
                    .Select(x => x.Points).Sum() + item.DraftItems
                    .Where(x => x.TeamName == item.MatchScore.AwayTeamName)
                    .Select(x => x.Points).Sum() ?? 0;

                var boxscoreViewModel = new BoxscoreDraftByMatchViewModel
                {
                    DraftRosterItems = item.DraftRosterItems.Select(x => new DraftRosterItemViewModel()
                    {
                        TeamName = x.TeamName,
                        PlayerName = x.PlayerName,
                        TeamId = x.TeamId,
                        LeagueId = x.LeagueId,
                        LeagueName = x.LeagueName,
                        PlayerId = x.PlayerId
                    }).ToList(),
                    LeagueId = leagueId,
                    MatchResultId = item.MatchScore.MatchId,
                    HomeDraftBoxscoreItems = item.DraftItems
                    .Where(x => x.TeamName == item.MatchScore.HomeTeamName)
                    .Select(x => new BoxscoreItemViewModel()
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
                    }
                    ).ToList(),
                    AwayDraftBoxscoreItems = item.DraftItems
                    .Where(x => x.TeamName == item.MatchScore.AwayTeamName)
                    .Select(x => new BoxscoreItemViewModel()
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
                    }
                    ).ToList(),
                    HomeExistingBoxscoreItems = item.ExistingItems
                    .Where(x => x.TeamName == item.MatchScore.HomeTeamName)
                    .Select(x => new BoxscoreItemViewModel()
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
                    }
                    ).ToList(),
                    AwayExistingBoxscoreItems = item.ExistingItems
                    .Where(x => x.TeamName == item.MatchScore.AwayTeamName)
                    .Select(x => new BoxscoreItemViewModel()
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
                    }
                    ).ToList(),
                    MissingPlayerItems = item.MissingPlayers.ToList(),
                    MatchResult = new MatchResultViewModel
                    {
                        MatchNo = item.MatchScore.MatchNo,
                        Round = item.MatchScore.Round,
                        Attendency = item.MatchScore.Attendency ?? 0,
                        AwayTeamName = item.MatchScore.AwayTeamName,
                        AwayTeamPoints = item.MatchScore.AwayTeamPoints ?? 0,
                        HomeTeamName = item.MatchScore.HomeTeamName,
                        HomeTeamPoints = item.MatchScore.HomeTeamPoints ?? 0,
                        Venue = item.MatchScore.Venue
                    },
                    AwayTeamPoints = realValuepointsByAwayTeam,
                    HomeTeamPoints = realValuePointsByHomeTeam,
                    AwayTeamMatch = realValuepointsByAwayTeam == item.MatchScore.AwayTeamPoints,
                    HomeTeamMatch = realValuePointsByHomeTeam == item.MatchScore.HomeTeamPoints,
                    AvailableDraftBoxscoreItems = item.DraftItems.Any(),
                    AvailableDraftRosterItems = item.DraftRosterItems.Any(),
                    AvailableMissingPlayerItems = item.MissingPlayers.Any(),
                };
                list.Add(boxscoreViewModel);
            }

            var boxscoreByRoundViewModel = new BoxscoreDraftByRoundViewModel
            {
                LeagueId = leagueId,
                Name = result.Value.LeagueName,
                RoundNo = roundNo,
                Matches = list
            };
            return View(boxscoreByRoundViewModel);
        }

    }
}
