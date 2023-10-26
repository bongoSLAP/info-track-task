using InfoScrape.Core.Enums;
using InfoScrape.Core.Interfaces;

namespace InfoScrape.Handlers;

public class RankSearchHandler : BaseSearchHandler, IRankSearchHandler
{
    public RankSearchHandler(IScraperFactory factory) : base(factory)
    {
        
    }

    public IEnumerable<int> GetRanks(SearchEngine engine, string search, string targetUrl)
    {
        var urls = ScrapeUrls(engine, search);
        targetUrl = NormalizeUrl(targetUrl);

        return urls.Select((value, index) => new { value, index })
            .Where(param => param.value.Contains(targetUrl))
            .Select(param => param.index + 1);
    }
}