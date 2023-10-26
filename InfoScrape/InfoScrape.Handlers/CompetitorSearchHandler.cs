using InfoScrape.Core.Enums;
using InfoScrape.Core.Interfaces;
namespace InfoScrape.Handlers;

public class CompetitorSearchHandler : BaseSearchHandler, ICompetitorSearchHandler
{
    public CompetitorSearchHandler(IScraperFactory factory) : base(factory)
    {
        
    }
    
    public IEnumerable<string> GetCompetitors(SearchEngine engine, string search, string targetUrl)
    {
        var urls = ScrapeUrls(engine, search).ToList();
        
        if (urls[0].Contains(targetUrl))
            return new List<string>() { urls[0] };
        
        var normalizedTargetUrl = NormalizeUrl(targetUrl);

        var competitors = new List<string>();
        var seenUrls = new HashSet<string>(); // get unique urls only
    
        foreach (var url in urls)
        {
            if (url.Contains(normalizedTargetUrl))
                break;

            if (!seenUrls.Add(url)) 
                continue;
            
            competitors.Add(url);

            if (competitors.Count == 10)
                break;
        }

        return competitors;
    }
}