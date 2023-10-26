using InfoScrape.Core.Enums;
using InfoScrape.Core.Interfaces;

namespace InfoScrape.Handlers;

public abstract class BaseSearchHandler : IBaseSearchHandler
{
    private readonly IScraperFactory _factory;

    protected BaseSearchHandler(IScraperFactory factory)
    {
        _factory = factory;
    }

    public IEnumerable<string> ScrapeUrls(SearchEngine engine, string search)
    {
        IScraper scraper = _factory.CreateScraper(engine);
        return scraper.Scrape(search);
    }

    public string NormalizeUrl(string targetUrl)
    {
        if (!targetUrl.StartsWith("http://") && !targetUrl.StartsWith("https://")) 
            return targetUrl;
        
        var targetUri = new Uri(targetUrl);
        return (targetUri.Host + targetUri.PathAndQuery).TrimEnd('/');
    }
}