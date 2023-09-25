using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Crawler.Models;

namespace OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts
{
    public interface IEuroleagueProcessor
    {
        Task<IReadOnlyList<(string Name, string Url)>> GetTeams(string leagueUrl,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<(int? No, string Name, string Position, decimal Height, DateTime DateOfBirth, string Nationality, DateTime Start, DateTime? End)>>
            GetRoster(string teamUrl,
                CancellationToken cancellationToken = default);
        Task<IReadOnlyList<(int? Round, string HomeTeamName, string AwayTeamName, int? HomeTeamPoints, int? AwayTeamPoints, DateTime? Date, int? MatchNo)>> GetRegularSeasonCalendar(int round, string calendarUrl, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<(int? Attendency, string Venue, int HomeTeamPoint, int AwayTeamPoint)>> GetMatchResult(int round,
            IEnumerable<string> matchUrls, CancellationToken cancellationToken = default);

        Task<(IReadOnlyList<PlayerScore>
            HomeTeam,
            IReadOnlyList<PlayerScore>
            AwayTeam)> GetBoxScore(string matchUrl, CancellationToken cancellationToken = default);
    }
}
