using AngleSharp;
using AngleSharp.Dom;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenData.Basetball.AbaLeague.Crawler.Configurations;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace OpenData.Basetball.AbaLeague.Crawler.Fetchers.Implementation
{
    public class DocumentFetcher : IDocumentFetcher
    {
        private readonly CrawlerSettings _crawlerSettings;
        private readonly ILogger<DocumentFetcher> _logger;

        public DocumentFetcher(IOptions<CrawlerSettings> options,ILogger<DocumentFetcher> logger)
        {
            _crawlerSettings = options.Value;
            _logger = logger;
        }
        public async Task<IDocument> FetchDocument(string url, CancellationToken cancellationToken)
        {
            IConfiguration configuration = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(configuration);
            IDocument document = await context
                .OpenAsync(url, cancellationToken);

            return document;
        }

        public async Task<IDocument> FetchDocumentBySelenium(string url, CancellationToken cancellationToken)
        {
            ChromeDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            try
            {

                wait.Until(driver => driver.FindElement(By.Id("onetrust-consent-sdk")));
            }
            catch (Exception ex)
            {

            }
            var config = Configuration.Default;
             var context = BrowsingContext.New(config);
             var doc = await context.OpenAsync(req => req.Content(driver.PageSource), cancellationToken);

             driver.Quit();
             return doc;
        }
    }
}
