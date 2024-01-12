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
        private readonly IEuroleagueProcessor _euroleagueProcessor;

        public LeagueService(IUnitOfWork unitOfWork,IWebPageProcessor webPageProcessor, IEuroleagueProcessor euroleagueProcessor)
        {
            _unitOfWork = unitOfWork;
            _webPageProcessor = webPageProcessor;
            _euroleagueProcessor = euroleagueProcessor;
        }
        public async Task<IEnumerable<League>> Get(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.LeagueRepository.GetAll(cancellationToken);
        }

        public async Task<League> Get(int id, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.LeagueRepository.Get(id,cancellationToken);
        }

        public async Task Add(LeagueItemDto league, CancellationToken cancellationToken = default)
        {
            var entity = new League()
            {
                OfficalName = league.OfficialName,
                ShortName = league.ShortName,
                StandingUrl = league.StandingUrl,
                BaseUrl = league.BaseUrl,
                BoxScoreUrl = league.BoxScoreUrl,
                CalendarUrl = league.CalendarUrl,
                MatchUrl = league.MatchUrl,
                RoundMatches = new List<RoundMatch>()
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

            var matchDayItems =
                new List<(int? Round, string HomeTeamName, string AwayTeamName, int HomeTeamPoints, int AwayTeamPoints,
                    DateTime? Date, int? MatchNo)>();


            var count = seasonResources.Count * 2;

            for (int i = 0; i < count; i++)
            {
                var rawUrl = league.BaseUrl+league.CalendarUrl;
                var url = string.Format(rawUrl, i+1);
                var items = await _webPageProcessor.GetRegularSeasonCalendar(url, cancellationToken);
                var newItems = items.Select(x => (((int?)i, x.HomeTeamName, x.AwayTeamName, x.HomeTeamPoints,
                    x.AwayTeamPoints, x.Date, x.MatchNo)));
                matchDayItems.AddRange((IEnumerable<(int? Round, string HomeTeamName, string AwayTeamName, int HomeTeamPoints, int AwayTeamPoints, DateTime? Date, int? MatchNo)>)newItems.ToList());
            }

            List<RoundMatchDto> matches = new List<RoundMatchDto>();

            foreach (var item in matchDayItems)
            {
                var homeTeam = seasonResources.FirstOrDefault(x => x.TeamName == item.HomeTeamName);
                var awayTeam = seasonResources.FirstOrDefault(x => x.TeamName == item.AwayTeamName);
                if (homeTeam == null || awayTeam == null)
                {
                    continue;
                }

                matches.Add(new RoundMatchDto(homeTeam.TeamId, awayTeam.TeamId, item.HomeTeamName, item.AwayTeamName,
                    item.HomeTeamPoints, item.AwayTeamPoints, item.Round??-1, item.MatchNo??-1, item.Date??DateTime.MinValue
                ));

            }

            return matches;
        }

        public async Task<IEnumerable<RoundMatchDto>> GetEuroleagueCalendarDraft(int leagueId, CancellationToken cancellationToken = default)
        {
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);
            var seasonResources = await _unitOfWork.SeasonResourcesRepository.SearchByLeague(leagueId, cancellationToken);

            var matchDayItems =
                new List<(int? Round, string HomeTeamName, string AwayTeamName, int? HomeTeamPoints, int? AwayTeamPoints,
                    DateTime? Date, int? MatchNo)>();


            var count = seasonResources.Count * 2;

            for (int i = 0; i < count; i++)
            {
                var rawUrl = league.BaseUrl + league.CalendarUrl;
                var url = string.Format(rawUrl, i + 1);
                matchDayItems.AddRange(await _euroleagueProcessor.GetRegularSeasonCalendar(i+1, url, cancellationToken));
            }

            List<RoundMatchDto> matches = new List<RoundMatchDto>();

            foreach (var item in matchDayItems)
            {
                var homeTeam = seasonResources.FirstOrDefault(x => x.TeamName == item.HomeTeamName);
                var awayTeam = seasonResources.FirstOrDefault(x => x.TeamName == item.AwayTeamName);
                if (homeTeam == null || awayTeam == null)
                {
                    continue;
                }

                matches.Add(new RoundMatchDto(homeTeam.TeamId, awayTeam.TeamId, item.HomeTeamName, item.AwayTeamName,
                    item.HomeTeamPoints, item.AwayTeamPoints, item.Round ?? -1, item.MatchNo ?? -1, item.Date ?? DateTime.MinValue
                ));

            }

            return matches;
        }

    

        public async Task<IEnumerable<RoundMatchDto>> GetEuroleagueCalendarSpecificRoundDraft(int leagueId, int round, CancellationToken cancellationToken = default)
        {
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);
            var seasonResources = await _unitOfWork.SeasonResourcesRepository.SearchByLeague(leagueId, cancellationToken);

            var matchDayItems =
                new List<(int? Round, string HomeTeamName, string AwayTeamName, int? HomeTeamPoints, int? AwayTeamPoints,
                    DateTime? Date, int? MatchNo)>();

            var rawUrl = league.BaseUrl + league.CalendarUrl;
            var url = string.Format(rawUrl, round);
            matchDayItems.AddRange(await _euroleagueProcessor.GetRegularSeasonCalendar(round, url, cancellationToken));
            

            List<RoundMatchDto> matches = new List<RoundMatchDto>();

            foreach (var item in matchDayItems)
            {
                var homeTeam = seasonResources.FirstOrDefault(x => x.TeamName == item.HomeTeamName);
                var awayTeam = seasonResources.FirstOrDefault(x => x.TeamName == item.AwayTeamName);
                if (homeTeam == null || awayTeam == null)
                {
                    continue;
                }

                matches.Add(new RoundMatchDto(homeTeam.TeamId, awayTeam.TeamId, item.HomeTeamName, item.AwayTeamName,
                    item.HomeTeamPoints, item.AwayTeamPoints, item.Round ?? -1, item.MatchNo ?? -1, item.Date ?? DateTime.MinValue
                ));

            }

            return matches;
        }

        public Task<IEnumerable<RoundMatchDto>> GetExistingCalendar(int leagueId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RoundMatchDto>> AddCalendar(int leagueId, IEnumerable<AddRoundMatchDto> entries,bool isOffSeason=false, CancellationToken cancellationToken = default)
        {
            List<RoundMatchDto> output = new List<RoundMatchDto>();
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);
            if ( league == null)
            {
                return Array.Empty<RoundMatchDto>();
            }

            foreach (var addRoundMatchDto in entries)
            {
                var homeTeam = await _unitOfWork.TeamRepository.Get(addRoundMatchDto.HomeTeamId, cancellationToken);
                var awayTeam = await _unitOfWork.TeamRepository.Get(addRoundMatchDto.AwayTeamId, cancellationToken);
                if (homeTeam == null || awayTeam == null) 
                {
                    continue;
                    
                }
                if (league.RoundMatches == null)
                {
                    league.RoundMatches = new List<RoundMatch>();
                }

                if (!isOffSeason)
                {
                    if (await _unitOfWork.CalendarRepository.Exist(leagueId, 
                            addRoundMatchDto.HomeTeamId,
                            addRoundMatchDto.AwayTeamId,
                            cancellationToken))
                    {
                        continue;
                    }
                }
               

                var newRoundItem = new RoundMatch()
                {
                    AwayTeam = awayTeam,
                    AwayTeamId = awayTeam.Id,
                    HomeTeam = homeTeam,
                    HomeTeamId = homeTeam.Id,
                    MatchNo = addRoundMatchDto.MatchNo,
                    Round = addRoundMatchDto.Round,
                    OffSeason = isOffSeason,
                    DateTime = addRoundMatchDto.
                        DateTime.ToUniversalTime(),
                };

                league.RoundMatches.Add(newRoundItem);

                await _unitOfWork.LeagueRepository.Update(league, cancellationToken);
                await _unitOfWork.Save();

                RoundMatchDto outputItem = new RoundMatchDto(newRoundItem.HomeTeamId??0, newRoundItem.AwayTeamId??0,
                    newRoundItem.HomeTeam.Name, newRoundItem.AwayTeam.Name, 0, 0, newRoundItem.Round,
                    newRoundItem.MatchNo, newRoundItem.DateTime);
                output.Add(outputItem);
            }

            return output;
        }
    }
}
