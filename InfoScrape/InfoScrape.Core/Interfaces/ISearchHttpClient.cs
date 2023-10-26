using HtmlAgilityPack;

namespace InfoScrape.Core.Interfaces;

public interface ISearchHttpClient
{
    HtmlDocument GetDocument(string url);
}