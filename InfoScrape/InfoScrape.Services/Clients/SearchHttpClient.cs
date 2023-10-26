using System.Net;
using HtmlAgilityPack;
using InfoScrape.Core.Interfaces;

namespace InfoScrape.Services.Clients;

public class SearchHttpClient : ISearchHttpClient
{
    private readonly HttpClient _http;
    
    public SearchHttpClient(IHttpClientFactory factory)
    {
        _http = factory.CreateClient();
        _http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36"); // trying to avoid rate limiting or IP ban
    }
    
    public HtmlDocument GetDocument(string url)
    {
        try
        {
            var html = _http.GetStringAsync(url).Result;
            var document = new HtmlDocument();
            document.LoadHtml(html);
            return document;
        }
        catch (AggregateException ex) when (ex.InnerException is HttpRequestException { StatusCode: HttpStatusCode.TooManyRequests })
        {
            throw new InvalidOperationException("Rate limit exceeded. Try again later.", ex);
        }
    }
}