using InfoScrape.Core.Enums;

namespace InfoScrape.Core.Interfaces;

public interface IRankSearchHandler
{
    IEnumerable<int> GetRanks(SearchEngine engine, string search, string targetUrl);
}