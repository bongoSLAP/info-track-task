using InfoScrape.Core.Enums;

namespace InfoScrape.Core.Models;

public class SearchRequest
{
    public SearchEngine Engine { get; set; }
    public string Search { get; set; } = string.Empty;
    public string? TargetUrl { get; set; }
}