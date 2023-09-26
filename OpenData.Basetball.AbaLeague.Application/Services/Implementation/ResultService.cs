using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basketball.AbaLeague.Application.DTOs;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Application.Services.Implementation
{
    public class ResultService:IResultService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebPageProcessor _webPageProcessor;
        private readonly IEuroleagueProcessor _euroleagueProcessor;

        public ResultService(IWebPageProcessor webPageProcessor,IUnitOfWork unitOfWork, IEuroleagueProcessor euroleagueProcessor)
        {
            _webPageProcessor = webPageProcessor;
            _euroleagueProcessor = euroleagueProcessor;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ResultDto>> GetResultsByRoundId(int leagueId, int roundId, CancellationToken cancellationToken = default)
        {
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);
            if (league == null)
            {
                return Array.Empty<ResultDto>();
            }

            var matches = await _unitOfWork.CalendarRepository.SearchByRound(leagueId, roundId, cancellationToken);
            List<ResultDto> resultSet = new List<ResultDto>();
            foreach (var roundMatch in matches)
            {
                var url =
                    string.Format(league.MatchUrl, roundMatch.MatchNo);

                var items = await _webPageProcessor.GetMatchResult(new List<string>(){url}, cancellationToken);
                var item = items.FirstOrDefault();

                resultSet.Add(new ResultDto(roundMatch.Id,roundMatch.HomeTeam.Name,roundMatch.AwayTeam.Name,item.Attendency,item.Venue,item.HomeTeamPoint,item.AwayTeamPoint));
            }

            return resultSet;
        }

        public async Task<IEnumerable<ResultDto>> GetResults(int leagueId, CancellationToken cancellationToken = default)
        {
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);
            if (league == null)
            {
                return Array.Empty<ResultDto>();
            }

            var matches = await _unitOfWork.CalendarRepository.SearchByLeague(leagueId, cancellationToken); 
            List<ResultDto> resultSet = new List<ResultDto>();
            foreach (var roundMatch in matches)
            {
                var url =
                    string.Format(league.MatchUrl, roundMatch.MatchNo);

                var items = await _webPageProcessor.GetMatchResult(new List<string>() { url }, cancellationToken);
                var item = items.FirstOrDefault();

                resultSet.Add(new ResultDto(roundMatch.Id, roundMatch.HomeTeam.Name, roundMatch.AwayTeam.Name, item.Attendency, item.Venue, item.HomeTeamPoint, item.AwayTeamPoint));
            }

            return resultSet;
        }

        public async Task<IEnumerable<ResultDto>> GetEuroleagueResults(int leagueId, CancellationToken cancellationToken = default)
        {
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);
            if (league == null)
            {
                return Array.Empty<ResultDto>();
            }

            var matches = await _unitOfWork.CalendarRepository.SearchByLeague(leagueId, cancellationToken);
            List<ResultDto> resultSet = new List<ResultDto>();
            if (matches == null)
            {
                return resultSet;
            }
            foreach (var roundMatch in matches)
            {
                var url =
                    string.Format(league.BaseUrl+league.MatchUrl, roundMatch.MatchNo);

                var items = await _euroleagueProcessor.GetMatchResult(new List<string>() { url }, cancellationToken);
                var item = items.FirstOrDefault();

                resultSet.Add(new ResultDto(roundMatch.Id, roundMatch.HomeTeam.Name, roundMatch.AwayTeam.Name, item.Attendency, item.Venue, item.HomeTeamPoint, item.AwayTeamPoint));
            }

            return resultSet;
        }
        public async Task<IEnumerable<ResultDto>> GetEuroleagueResults(int leagueId, int matchNo, CancellationToken cancellationToken = default)
        {
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);
            if (league == null)
            {
                return Array.Empty<ResultDto>();
            }

            var matches = await _unitOfWork.CalendarRepository.SearchByLeague(leagueId, cancellationToken);
            List<ResultDto> resultSet = new List<ResultDto>();
            if (matches == null)
            {
               return  resultSet;
            }
            var url =
                string.Format(league.BaseUrl + league.MatchUrl, matchNo);

            var items = await _webPageProcessor.GetMatchResult(new List<string>() { url }, cancellationToken);
            var item = items.FirstOrDefault();

            resultSet.Add(new ResultDto(matchNo, string.Empty, string.Empty, item.Attendency, item.Venue, item.HomeTeamPoint, item.AwayTeamPoint));
            

            return resultSet;
        }

        public async Task<IEnumerable<ResultDto>> Add(int leagueId, IEnumerable<AddResultDto> entries, CancellationToken cancellationToken = default)
        {
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);
            if (league == null)
            {
                return Array.Empty<ResultDto>();
            }

            List<ResultDto> output = new List<ResultDto>();

            foreach (var addResultDto in entries)
            {
                if (!await _unitOfWork.ResultRepository.Exist(addResultDto.MatchRoundId, cancellationToken))
                {
                    var calendarItem =
                        await _unitOfWork.CalendarRepository.Get(addResultDto.MatchRoundId, cancellationToken);
                    var newItem = new Result()
                    {
                        RoundMatch = calendarItem,
                        RoundMatchId = calendarItem.Id,
                        Attendency = addResultDto.Attendency ?? -1,
                        Venue = addResultDto.Venue,
                        AwayTeamPoint = addResultDto.AwayTeamPoint,
                        HomeTeamPoints = addResultDto.HomeTeamPoint
                    };
                    await _unitOfWork.ResultRepository.Add(newItem, cancellationToken);

                    await _unitOfWork.Save();

                    var outputItem = new ResultDto(newItem.RoundMatchId, "", "", newItem.Attendency, newItem.Venue,
                        newItem.HomeTeamPoints, newItem.AwayTeamPoint);
                    output.Add(outputItem);
                }
            }

            return output;
        }
    }
}
