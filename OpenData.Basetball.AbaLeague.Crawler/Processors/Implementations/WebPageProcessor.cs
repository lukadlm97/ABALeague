using AngleSharp.Html.Dom;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Implementation;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Crawler.Processors.Implementations
{
    public class WebPageProcessor:IWebPageProcessor
    {
        private readonly ILeagueFetcher _leagueFetcher;

        public WebPageProcessor(ILeagueFetcher leagueFetcher)
        {
            _leagueFetcher = leagueFetcher;
        }
        public async Task<IReadOnlyList<(string name,string url)>> GetTeams(string leagueUrl, CancellationToken cancellationToken = default)
        {
            var webDocument = await _leagueFetcher
                .FetchTeams(leagueUrl, cancellationToken);

            var teams = new List<(string,string)>();
            var teamElements = webDocument.QuerySelectorAll("table > tbody > tr");

            foreach (var teamElement in teamElements)
            {
                var name = teamElement
                    .QuerySelectorAll("td")[1]
                    .QuerySelectorAll("a")[0]
                    .InnerHtml
                    .Trim();

                var url = teamElement
                    .QuerySelectorAll("td")[1]
                    .QuerySelectorAll("a")[0]
                    .GetAttribute("href")
                    .Trim();
              

                teams.Add((name, url));
            }

            return teams;

        }
    }
}
