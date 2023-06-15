using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;

namespace OpenData.Basketball.AbaLeague.Application.Services.Implementation
{
    public class SeasonResourcesService : ISeasonResourcesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SeasonResourcesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SeasonResourceDto>> Get(CancellationToken cancellationToken = default)
        {
            return (await _unitOfWork.SeasonResourcesRepository
                .GetAll(cancellationToken)).Select(x=>new SeasonResourceDto(0,x.LeagueId,x.League.OfficalName,x.TeamId,x.Team.Name,x.League.BaseUrl+""+x.TeamSourceUrl));
        }

        public async Task<IEnumerable<SeasonResources>> Get(int teamId, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.SeasonResourcesRepository
                .SearchByTeam(teamId, cancellationToken);

        }

        public async Task<IEnumerable<SeasonResourceDto>> GetTeams(int leagueId,
            CancellationToken cancellationToken = default)
        {
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);

            var seasonResources = await _unitOfWork.SeasonResourcesRepository
                .SearchByLeague(leagueId, cancellationToken);

            return seasonResources.Select(x => new SeasonResourceDto(leagueId, league.Id, league.OfficalName, x.TeamId,
                x.Team.Name, league.BaseUrl + "" + x.TeamSourceUrl));
        }


        public async Task<SeasonResources> Add(AddSeasonResourceDto seasonResourceDto,
            CancellationToken cancellationToken = default)
        {
            var league = await _unitOfWork.LeagueRepository
                .Get(seasonResourceDto.LeagueId, cancellationToken);
            var team = await _unitOfWork.TeamRepository
                .Get(seasonResourceDto.TeamId, cancellationToken);

            if (league == null || team == null)
            {
                throw new ArgumentException();
            }

            var season = new SeasonResources()
            {
                Team = team,
                TeamId = team.Id,
                LeagueId = league.Id,
                League = league,
                TeamSourceUrl = seasonResourceDto.Url
            };

            await _unitOfWork.SeasonResourcesRepository.Add(season, cancellationToken);
            await _unitOfWork.Save();

            return season;
        }

        public async Task<IEnumerable<SeasonResources>> Add(IEnumerable<AddSeasonResourceDto> seasonResourceDto, CancellationToken cancellationToken = default)
        {
            List<SeasonResources> seasonResources = new List<SeasonResources>();
            foreach (var resourceDto in seasonResourceDto)
            {
                var league = await _unitOfWork.LeagueRepository
                    .Get(resourceDto.LeagueId, cancellationToken);
                var team = await _unitOfWork.TeamRepository
                    .Get(resourceDto.TeamId, cancellationToken);

                if (league == null || team == null)
                {
                    throw new ArgumentException();
                }

                var season = new SeasonResources()
                {
                    Team = team,
                    TeamId = team.Id,
                    LeagueId = league.Id,
                    League = league,
                    TeamSourceUrl = resourceDto.Url
                };

                seasonResources.Add(season);
            }

            await _unitOfWork.SeasonResourcesRepository.Add(seasonResources, cancellationToken);
            await _unitOfWork.Save();

            return seasonResources;

        }

        public async Task<SeasonResources> UpdateUrl(int resourceId, string url, CancellationToken cancellationToken = default)
        {
            var resource = await _unitOfWork.SeasonResourcesRepository
                .Get(resourceId, cancellationToken);

            if (resource == null)
            {
                throw new ArgumentException();
            }
            resource.TeamSourceUrl=url;
            await _unitOfWork.SeasonResourcesRepository
                .Update(resource, cancellationToken);
            await _unitOfWork.Save();

            return resource;
        }
    }
}
