using InfoScrape.Core.Enums;

namespace InfoScrape.Core.Interfaces;

public interface IScraperFactory
{
    IScraper CreateScraper(SearchEngine engine);
}