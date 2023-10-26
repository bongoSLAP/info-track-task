using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using InfoScrape.Core.Interfaces;
using OpenQA.Selenium;

namespace InfoScrape.Services.Scrapers;

public class DuckDuckGoScraper : IScraper, IDisposable
{
    private readonly ISearchSeleniumClient _selenium;
    private const string BaseUrl = "https://duckduckgo.com/?va=o&t=hx&q=";
    private const string ResultUrlDomPath = "//div[@class='OQ_6vPwNhCeusNiEDcGp']";
    private const int MaxRetries = 3;

    public DuckDuckGoScraper(ISearchSeleniumClient selenium)
    {
        _selenium = selenium;
    }

    public IEnumerable<string> Scrape(string search)
    {
        var searchUrl = $"{BaseUrl}{WebUtility.UrlEncode(search)}&ia=web";
        HtmlDocument? document = FetchDocumentRetry(searchUrl);
        
        if (document == null)
            throw new InvalidOperationException($"Failed to fetch the document from '{searchUrl}'.");

        HtmlNodeCollection? resultNodes = document.DocumentNode.SelectNodes(ResultUrlDomPath);
        
        if (resultNodes == null || !resultNodes.Any())
            throw new InvalidOperationException($"Failed to fetch the document from '{searchUrl}'.");

        var urls = ParseUrls(resultNodes).ToList();

        if (!urls.Any())
            throw new InvalidOperationException($"Failed to fetch the document from '{searchUrl}'.");

        return urls;
    }

    private HtmlDocument? FetchDocumentRetry(string url)
    {
        HtmlDocument? document = null;
        var retries = 0;
        while (retries < MaxRetries)
        {
            try
            {
                document = _selenium.GetDocument(url);
                break;
            }
            catch (StaleElementReferenceException)
            {
                retries++;
                if (retries == MaxRetries)
                    throw; // StaleElementReferenceException
            }
        }
        
        _selenium.Dispose();

        return document;
    }

    private static IEnumerable<string> ParseUrls(HtmlNodeCollection nodes)
    {
        var urlList = new List<string>();
        
        foreach (var node in nodes)
        {
            var innerHtml = node.InnerHtml;
            
            if (string.IsNullOrEmpty(innerHtml))
                throw new InvalidOperationException("No urls were found in search results.");
            
            const string pattern = @"<span[^>]*>([^<]*)<\/span>";
            var match = Regex.Match(innerHtml, pattern);

            if (!match.Success)
                throw new InvalidOperationException("No urls were found in search results.");
            
            var hrefValue = match.Groups[1].Value;
            urlList.Add(hrefValue);
        }
  
        return urlList;
    }

    public void Dispose()
    {
        _selenium?.Dispose();
        GC.SuppressFinalize(this);
    }
}