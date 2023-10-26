using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using InfoScrape.Core.Interfaces;

namespace InfoScrape.Services.Scrapers;

public class GoogleScraper : IScraper
{
    private readonly ISearchHttpClient _http;
    private const string BaseUrl = "https://www.google.co.uk/search?num=100&q=";
    private const string ResultUrlDomPath = "//div[@class='yuRUbf']//a[@jsname='UWckNb']"; 

    public GoogleScraper(ISearchHttpClient http)
    {
        _http = http;
    }

    public IEnumerable<string> Scrape(string search)
    {
        var searchUrl = $"{BaseUrl}{WebUtility.UrlEncode(search)}";
        HtmlDocument document = _http.GetDocument(searchUrl);
        HtmlNodeCollection? resultNodes = document.DocumentNode.SelectNodes(ResultUrlDomPath);
        
        if (resultNodes == null || !resultNodes.Any())
            throw new InvalidOperationException($"Failed to fetch the document from '{searchUrl}'.");

        var urls = ParseHrefs(resultNodes).ToList();

        if (!urls.Any())
            throw new InvalidOperationException($"Failed to fetch the document from '{searchUrl}'.");

        return urls;
    }

    private static IEnumerable<string> ParseHrefs(HtmlNodeCollection nodes)
    {
        var urlList = new List<string>();
        
        foreach (var node in nodes)
        {
            string outerHtml = node.OuterHtml;
            
            if (string.IsNullOrEmpty(outerHtml))
                throw new InvalidOperationException("No urls were found in search results.");
            
            const string pattern = @"href=""([^""]+)""";
            var match = Regex.Match(outerHtml, pattern);

            if (!match.Success)
                throw new InvalidOperationException("No urls were found in search results.");
            
            string hrefValue = match.Groups[1].Value;
            urlList.Add(hrefValue);
        }

        return urlList;
    }
}