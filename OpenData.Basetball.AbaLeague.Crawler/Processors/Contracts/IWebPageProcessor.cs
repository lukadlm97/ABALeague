using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts
{
    public interface IWebPageProcessor
    {
        Task<IReadOnlyList<Team>> GetTeams(string leagueUrl,
            CancellationToken cancellationToken=default);
    }
}
