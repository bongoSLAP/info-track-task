using HtmlAgilityPack;

namespace InfoScrape.Core.Interfaces;

public interface IScraper
{
    IEnumerable<string> Scrape(string search);
}