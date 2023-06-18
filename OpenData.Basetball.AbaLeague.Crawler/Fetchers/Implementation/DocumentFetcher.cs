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
    public class DocumentFetcher:IDocumentFetcher
    {
        private readonly CrawlerSettings _crawlerSettings;
        private readonly ILogger<DocumentFetcher> _logger;

        public DocumentFetcher(IOptions<CrawlerSettings> options,ILogger<DocumentFetcher> logger)
        {
            _crawlerSettings = options.Value;
            _logger = logger;
        }
        public async Task<IDocument> FetchDocument(string leagueUrl, CancellationToken cancellationToken)
        {
            IConfiguration configuration = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(configuration);
            IDocument document = await context
                .OpenAsync(leagueUrl,cancellationToken);

            return document;
        }

       
    }
}
