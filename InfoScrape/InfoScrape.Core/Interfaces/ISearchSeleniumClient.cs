using HtmlAgilityPack;

namespace InfoScrape.Core.Interfaces;

public interface ISearchSeleniumClient
{
    HtmlDocument GetDocument(string url);
    void Dispose();
}