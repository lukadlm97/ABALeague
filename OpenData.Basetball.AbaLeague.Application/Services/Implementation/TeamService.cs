using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Implementations;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;

namespace OpenData.Basketball.AbaLeague.Application.Services.Implementation
{
    public class TeamService:ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebPageProcessor _webPageProcessor;

        public TeamService(IUnitOfWork unitOfWork,IWebPageProcessor webPageProcessor)
        {
            _unitOfWork = unitOfWork;
            _webPageProcessor = webPageProcessor;
        }
        public async Task<(IEnumerable<(Team,Team)> existingResulution,IEnumerable<Team> newly)> Get(int leagueId, CancellationToken cancellationToken)
        {
            var teams = await GetExisting(cancellationToken);
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);
            var leagueTeams = await _webPageProcessor.GetTeams(league.StandingUrl, cancellationToken);

            List<(Team,Team)> existingTeams = new List<(Team, Team)>();
            List<Team> newTeams = new List<Team>();

            foreach (var leagueTeam in leagueTeams)
            {
                if (teams!=null && teams.Any(x => leagueTeam.Name.ToLower()
                        .Contains(x.Name.ToLower())))
                {
                    var existingTeam = teams
                            .FirstOrDefault(x => leagueTeam.Name.ToLower()
                            .Contains(x.Name.ToLower()))

                        ;
                    if (existingTeam != null)
                    {

                        existingTeams.Add((existingTeam,leagueTeam));
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
                else
                {
                    newTeams.Add(leagueTeam);
                }
            }
            return (existingTeams,newTeams);
        }

        public async Task<IEnumerable<Team>> GetExisting(CancellationToken cancellationToken)
        {
            return (await _unitOfWork.TeamRepository.GetAll(cancellationToken));


        }

        public async Task Add(Team team, CancellationToken cancellationToken)
        {
            await _unitOfWork.TeamRepository.Add(team, cancellationToken);
            await _unitOfWork.Save();
        }

        public async Task Add(IEnumerable<Team> teams, CancellationToken cancellationToken)
        {
            await _unitOfWork.TeamRepository.Add(teams, cancellationToken);
            await _unitOfWork.Save();
        }

        public async Task Update(Team team, CancellationToken cancellationToken)
        {
            var existingTeam = await _unitOfWork.TeamRepository.Get(team.Id,cancellationToken);
            
            if (existingTeam != null)
            {
                throw new ArgumentException();
            }

            existingTeam.Name=team.Name;
            existingTeam.ShortCode=team.ShortCode;
           //TODO existingTeam.CountryId=team.ShortCode;
           await  _unitOfWork.TeamRepository.Update(existingTeam,cancellationToken);
           await  _unitOfWork.Save();
        }
    }
}
