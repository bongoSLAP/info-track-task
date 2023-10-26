using InfoScrape.Core.Enums;
using InfoScrape.Core.Interfaces;
using InfoScrape.Services.Scrapers;

namespace InfoScrape.Services.Factories;

public class ScraperFactory : IScraperFactory
{
    private readonly ISearchHttpClient _http;
    private readonly ISearchSeleniumClient _selenium;
    
    public ScraperFactory(ISearchHttpClient http, ISearchSeleniumClient selenium)
    {
        _http = http;
        _selenium = selenium;
    }
    
    public IScraper CreateScraper(SearchEngine engine)
    {
        return engine switch
        {
            SearchEngine.Google => new GoogleScraper(_http),
            SearchEngine.DuckDuckGo => new DuckDuckGoScraper(_selenium),
            _ => throw new ArgumentException("Search engine not supported")
        };
    }
}