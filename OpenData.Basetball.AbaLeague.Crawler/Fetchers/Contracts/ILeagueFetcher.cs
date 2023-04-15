using AngleSharp.Dom;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts
{
    public interface ILeagueFetcher
    {
        Task<IDocument> FetchTeams(string leagueUrl,CancellationToken cancellationToken);
        Task<IDocument> FetchPlayers(string leagueUrl, string team,
            CancellationToken cancellationToken);
        Task<IDocument> FetchSchedule(string leagueUrl, CancellationToken cancellationToken);
        Task<IDocument> FetchStatistic(string leagueUrl, CancellationToken cancellationToken);
    }
}
