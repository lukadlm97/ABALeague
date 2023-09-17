using AngleSharp.Dom;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts
{
    public interface IDocumentFetcher
    {
        Task<IDocument> FetchDocument(string url ,CancellationToken cancellationToken);
        Task<IDocument> FetchDocumentBySelenium(string url, CancellationToken cancellationToken);
    }
}
