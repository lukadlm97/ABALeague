using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Application.DTOs.Round;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Application.Services.Implementation
{
    public class LeagueService:ILeagueService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebPageProcessor _webPageProcessor;

        public LeagueService(IUnitOfWork unitOfWork,IWebPageProcessor webPageProcessor)
        {
            _unitOfWork = unitOfWork;
            _webPageProcessor = webPageProcessor;
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

        public async Task<IEnumerable<RoundMatchDto>> GetCalendarDraft(int leagueId, CancellationToken cancellationToken = default)
        {
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);
            var seasonResources = await _unitOfWork.SeasonResourcesRepository.SearchByLeague(leagueId, cancellationToken);
            var teams = _unitOfWork.TeamRepository.Get(seasonResources.Select(x => x.TeamId), cancellationToken);

            var matchDayItems= await _webPageProcessor.GetRegularSeasonCalendar(league.CalendarUrl, cancellationToken);

            List<RoundMatchDto> matches = new List<RoundMatchDto>();

            foreach (var item in matchDayItems)
            {
                var homeTeam = teams.FirstOrDefault(x => x.Name == item.HomeTeamName);
                var awayTeam = teams.FirstOrDefault(x => x.Name == item.AwayTeamName);
                if (homeTeam == null || awayTeam == null)
                {
                    continue;
                }

                matches.Add(new RoundMatchDto(homeTeam.Id, awayTeam.Id, item.HomeTeamName, item.AwayTeamName,
                    item.HomeTeamPoints, item.AwayTeamPoints, item.Round??-1, item.MatchNo??-1, item.Date??DateTime.MinValue
                ));

            }

            return matches;
        }

        public Task<IEnumerable<RoundMatchDto>> GetExistingCalendar(int leagueId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RoundMatchDto>> AddCalendar(int leagueId, IEnumerable<AddRoundMatchDto> entries, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
