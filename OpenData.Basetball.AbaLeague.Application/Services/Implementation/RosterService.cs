
using System.Formats.Asn1;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;

namespace OpenData.Basketball.AbaLeague.Application.Services.Implementation
{
    public class RosterService : IRosterService
    {
        private readonly IWebPageProcessor _webPageProcessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPlayerService _playerService;

        public RosterService(IWebPageProcessor webPageProcessor,IUnitOfWork unitOfWork, IPlayerService playerService)
        {
            _webPageProcessor = webPageProcessor;
            _unitOfWork = unitOfWork;
            _playerService = playerService;
        }


        public async Task<IEnumerable<RosterEntryDto>> GetDraftRoster(int teamId,int leagueId, CancellationToken cancellationToken = default)
        {
            var league = await _unitOfWork.LeagueRepository.Get(leagueId,cancellationToken);
            var seasonResource = await _unitOfWork.SeasonResourcesRepository.SearchByTeam(teamId, cancellationToken);
            var singleSeasonResoure = seasonResource.FirstOrDefault();

            var roster = await _webPageProcessor.GetRoster(league.BaseUrl+""+ singleSeasonResoure.TeamSourceUrl, cancellationToken);
            List<RosterEntryDto> outputCollection = new List<RosterEntryDto>();
            foreach (var rosterItem in roster)
            {
                var natioanality =
                    await _unitOfWork.CountryRepository.GetById(rosterItem.Nationality, cancellationToken);
                outputCollection.Add(new RosterEntryDto( rosterItem.Name, MapToEnum(rosterItem.Position), rosterItem.Height, rosterItem.DateOfBirth, natioanality,natioanality?.Id));
            }

            return outputCollection;

        }

        public async Task<IEnumerable<RosterEntryDto>> GetWholeDraftRoster(int leagueId, CancellationToken cancellationToken = default)
        {
            var teams = await _unitOfWork.SeasonResourcesRepository.SearchByLeague(leagueId, cancellationToken);
            List<RosterEntryDto> outputCollection = new List<RosterEntryDto>();

            foreach (var team in teams)
            {
                var rosterForTeam = await GetDraftRoster(team.TeamId, team.LeagueId, cancellationToken);
                outputCollection.AddRange(rosterForTeam);
            }

            return outputCollection;
        }

        public async Task<IEnumerable<RosterItemDto>> GetWholeRosterItemDraftRoster(int leagueId, CancellationToken cancellationToken = default)
        {
            var teams = await _unitOfWork.SeasonResourcesRepository.SearchByLeague(leagueId, cancellationToken);
            var players = await _unitOfWork.PlayerRepository.GetAll(cancellationToken);
            List<(int,RosterEntryDto)> list = new List<(int, RosterEntryDto)>();
            List<RosterItemDto> outputCollection = new List<RosterItemDto>();

            foreach (var team in teams)
            {
                var rosterForTeam = await GetDraftRoster(team.TeamId, team.LeagueId, cancellationToken);

                list.AddRange(rosterForTeam.Select(x=>(team.TeamId,x)));
            }

            foreach (var (teamId,rosterItem) in list)
            {
                var player = players.FirstOrDefault(x => x.Name == rosterItem.Name);
                if (player == null)
                {
                    var items = await _playerService.Add(new AddPlayerDto(rosterItem.Name, rosterItem.Position, (int)rosterItem.Height*100,
                        rosterItem.DateOfBirth, rosterItem.NationalityId),cancellationToken);
                    if (!items.Any())
                    {
                        throw new Exception();
                    }

                    players = await _unitOfWork.PlayerRepository.GetAll(cancellationToken);
                    player = players.FirstOrDefault(x => x.Name == rosterItem.Name);
                }
                outputCollection.Add(new RosterItemDto(player.Id,leagueId,teamId));
            }

            return outputCollection;
        }

        public async Task<IEnumerable<DraftRosterEntry>> Get(int leagueId, int teamId, CancellationToken cancellationToken = default)
        {
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);
            var seasonResource = await _unitOfWork.SeasonResourcesRepository.SearchByTeam(teamId, cancellationToken);
            var singleSeasonResoure = seasonResource.FirstOrDefault();

            var roster = await _webPageProcessor.GetRoster(league.BaseUrl + "" + singleSeasonResoure.TeamSourceUrl, cancellationToken);
            List<DraftRosterEntry> outputCollection = new List<DraftRosterEntry>();
            var players = await _unitOfWork.PlayerRepository.GetAll(cancellationToken);
            foreach (var rosterItem in roster)
            {
                var player = players.FirstOrDefault(x=>x.Name==rosterItem.Name);
                if (player != null)
                {
                    var newEntry = new DraftRosterEntry(player.Id, player.Name, leagueId, league.OfficalName);
                    outputCollection.Add(newEntry);
                }
            }

            return outputCollection;

        }

        public async Task<IEnumerable<DraftRosterEntry>> Add(int teamId, IEnumerable<DraftRosterEntry> entries, CancellationToken cancellationToken = default)
        {
            var team = await _unitOfWork.TeamRepository.Get(teamId, cancellationToken);
            if (team.RosterItems == null)
            {
                team.RosterItems = new List<RosterItem>();
            }

            foreach (var draftRosterEntry in entries)
            {
                if (team.RosterItems.Any(x =>
                        x.LeagueId == draftRosterEntry.LeagueId &&
                        x.PlayerId == draftRosterEntry.PlayerId &&
                        x.EndOfActivePeriod ==null))
                {
                    continue;
                }

                var league = await _unitOfWork.LeagueRepository.Get(draftRosterEntry.LeagueId, cancellationToken);
                var player = await _unitOfWork.PlayerRepository.Get(draftRosterEntry.PlayerId, cancellationToken);

                var newEntry = new RosterItem()
                {
                    LeagueId = league.Id,
                    PlayerId = player.Id,
                    League = league,
                    Player = player,
                    DateOfInsertion = DateTime.Now.ToUniversalTime()
                };


                team.RosterItems.Add(newEntry);

            }

             await _unitOfWork.TeamRepository.Update(team, cancellationToken);
             await _unitOfWork.Save();

             return team.RosterItems.Select(x =>
                 new DraftRosterEntry(x.PlayerId, x.Player.Name, x.LeagueId, x.League.OfficalName));
        }

        public async Task<IEnumerable<int>> Add(IEnumerable<AddRosterItemDto> entries, CancellationToken cancellationToken = default)
        {
            var teams = await _unitOfWork.TeamRepository.GetAll(cancellationToken);
            

            foreach (var draftRosterEntry in entries)
            {
                var team = teams.FirstOrDefault(x => x.Id == draftRosterEntry.TeamId);
                if (team == null)
                {
                    continue;
                }
                if (team.RosterItems == null)
                {
                    team.RosterItems = new List<RosterItem>();
                }
                if (team.RosterItems.Any(x =>
                        x.LeagueId == draftRosterEntry.LeagueId &&
                        x.PlayerId == draftRosterEntry.PlayerId &&
                        x.EndOfActivePeriod == null))
                {
                    continue;
                }

                var league = await _unitOfWork.LeagueRepository.Get(draftRosterEntry.LeagueId, cancellationToken);
                var player = await _unitOfWork.PlayerRepository.Get(draftRosterEntry.PlayerId, cancellationToken);

                var newEntry = new RosterItem()
                {
                    LeagueId = league.Id,
                    PlayerId = player.Id,
                    League = league,
                    Player = player,
                    DateOfInsertion = DateTime.Now.ToUniversalTime()
                };


                team.RosterItems.Add(newEntry);

                await _unitOfWork.TeamRepository.Update(team, cancellationToken);
                await _unitOfWork.Save();
            }

            

            return new List<int>();
        }

        PositionEnum MapToEnum(string value)
        {
            switch (value.Trim().ToLower())
            {
                case "shooting guard":
                    return PositionEnum.ShootingGuard; 
                case "center":
                    return PositionEnum.Center;
                case "power forward":
                    return PositionEnum.PowerForward;
                case "guard":
                    return PositionEnum.Guard;
                case "forward":
                    return PositionEnum.Forward;


                default:
                    return PositionEnum.Coach;
            }
        }
    }
}

