using InfoScrape.Core.Enums;

namespace InfoScrape.Core.Interfaces;

public interface ICompetitorSearchHandler
{
    public IEnumerable<string> GetCompetitors(SearchEngine engine, string search, string targetUrl);
}