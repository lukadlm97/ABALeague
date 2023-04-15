

using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;

namespace OpenData.Basketball.AbaLeague.Application.Services.Implementation
{
    public class LeagueService:ILeagueService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeagueService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;


        }
        public async Task<IEnumerable<League>> Get(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.LeagueRepository.GetAll(cancellationToken);
        }

        public async Task<League> Get(int id, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.LeagueRepository.Get(id,cancellationToken);
        }

        public async Task Add(LeagueDto league, CancellationToken cancellationToken = default)
        {
            var entity = new League()
            {
                OfficalName = league.OfficialName,
                Season = league.Season,
                ShortName = league.ShortName,
                StandingUrl = league.StandingUrl
            };
            await _unitOfWork.LeagueRepository.Add(entity,cancellationToken);
            await _unitOfWork.Save();
        }

        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.LeagueRepository
                .Get(id, cancellationToken);

            if (entity == null)
            {
                throw new ArgumentException("");
            }

            await _unitOfWork.LeagueRepository.Delete(entity, cancellationToken);
            await _unitOfWork.Save();
        }
    }
}
