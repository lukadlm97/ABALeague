using OpenData.Basetball.AbaLeague.Crawler.Models;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts
{
    public interface IWebPageProcessor
    {
        Task<IReadOnlyList<(string Name, string Url)>> GetTeams(string leagueUrl,
            CancellationToken cancellationToken=default);
        Task<IReadOnlyList<(int? No, string Name,string Position,decimal Height,DateTime DateOfBirth, string Nationality, DateTime Start, DateTime? End)>> 
            GetRoster(string teamUrl,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<(string HomeTeamName, string AwayTeamName, int? HomeTeamPoints, int? AwayTeamPoints, DateTime? Date, int? MatchNo, int? Round)>> GetRegularSeasonCalendar(string calendarUrl,CancellationToken  cancellationToken=default);
        Task<IReadOnlyList<(string HomeTeamName, string AwayTeamName, int? HomeTeamPoints, int? AwayTeamPoints, DateTime? Date, int? MatchNo, int? Round)>> GetRegularSeasonCalendar(int round, string calendarUrl, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<(int? MatchNo, int? Attendency, string? Venue, int? HomeTeamPoint, int? AwayTeamPoint)>> 
            GetMatchScores(IEnumerable<(int matchNo, string url)> matchResources, CancellationToken cancellationToken = default);

        Task<(IReadOnlyList<PlayerScore> 
            HomeTeam, 
            IReadOnlyList<PlayerScore> 
            AwayTeam)> GetBoxScore(string matchUrl, CancellationToken  cancellationToken=default);
    }
}
