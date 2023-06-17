using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenData.Basetball.AbaLeague.Crawler.Configurations;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Crawler.Fetchers.Implementation
{
    public class LeagueFetcher:ILeagueFetcher
    {
        private readonly CrawlerSettings _crawlerSettings;
        private readonly ILogger<LeagueFetcher> _logger;

        public LeagueFetcher(IOptions<CrawlerSettings> options,ILogger<LeagueFetcher> logger)
        {
            _crawlerSettings = options.Value;
            _logger = logger;
        }
        public async Task<IDocument> FetchTeams(string leagueUrl, CancellationToken cancellationToken)
        {
            IConfiguration configuration = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(configuration);
            IDocument document = await context
                .OpenAsync(leagueUrl,cancellationToken);

            return document;
        }

        public async Task<IDocument> FetchPlayers(string teamUrl, CancellationToken cancellationToken)
        {
            IConfiguration configuration = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(configuration);
            IDocument document = await context
                .OpenAsync(teamUrl, cancellationToken);

            return document;
        }

        public async Task<IDocument> FetchSchedule(string leagueUrl, CancellationToken cancellationToken)
        {
            IConfiguration configuration = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(configuration);
            IDocument document = await context
                .OpenAsync(leagueUrl, cancellationToken);

            return document;
        }

        public Task<IDocument> FetchStatistic(string leagueUrl, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
