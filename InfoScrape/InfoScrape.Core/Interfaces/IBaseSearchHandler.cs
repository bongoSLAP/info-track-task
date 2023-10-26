using InfoScrape.Core.Enums;

namespace InfoScrape.Core.Interfaces;

public interface IBaseSearchHandler
{
    IEnumerable<string> ScrapeUrls(SearchEngine engine, string search);
    string NormalizeUrl(string targetUrl);
}