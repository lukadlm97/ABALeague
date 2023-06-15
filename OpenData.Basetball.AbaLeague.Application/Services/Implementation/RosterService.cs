
using System.Formats.Asn1;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;

namespace OpenData.Basketball.AbaLeague.Application.Services.Implementation
{
    public class RosterService : IRosterService
    {
        private readonly IWebPageProcessor _webPageProcessor;
        private readonly IUnitOfWork _unitOfWork;

        public RosterService(IWebPageProcessor webPageProcessor,IUnitOfWork unitOfWork)
        {
            _webPageProcessor = webPageProcessor;
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<RosterEntryDto>> GetDraftRoster(int teamId,int leagueId, CancellationToken cancellationToken = default)
        {
            var team = await _unitOfWork.LeagueRepository.Get(leagueId,cancellationToken);
            var seasonResource = await _unitOfWork.SeasonResourcesRepository.SearchByTeam(teamId, cancellationToken);
            var singleSeasonResoure = seasonResource.FirstOrDefault();

            var roster = await _webPageProcessor.GetRoster(team.BaseUrl+""+ singleSeasonResoure.TeamSourceUrl, cancellationToken);

            return roster.Select(x =>
                new RosterEntryDto(x.No, x.Name, x.Position, x.height, x.DateOfBirth, x.Nationality));

        }
    }
}

