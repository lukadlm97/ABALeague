using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts
{
    public interface IWebPageProcessor
    {
        Task<IReadOnlyList<(string Name, string Url)>> GetTeams(string leagueUrl,
            CancellationToken cancellationToken=default);
        Task<IReadOnlyList<(int? No, string Name,string Position,decimal height,DateTime DateOfBirth, string Nationality)>> 
            GetRoster(string teamUrl,
            CancellationToken cancellationToken = default);
    }
}
