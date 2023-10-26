using InfoScrape.Core.Interfaces;
using InfoScrape.Core.Models;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;

namespace InfoScrape.Controllers;

public class CompetitorSearchController : BaseController
{
    private readonly ICompetitorSearchHandler _handler;
    
    public CompetitorSearchController(ICompetitorSearchHandler handler)
    {
        _handler = handler;
    }
    
    [HttpPost("Search/Competitor")]
    public IActionResult List([FromBody] SearchRequest? request)
    {
        if (!IsRequestValid(request))
            return BadRequest("Request object is invalid.");
        
        try
        {
            var targetUrl = request!.TargetUrl ?? "https://www.infotrack.co.uk";
            IEnumerable<string> competitors = _handler.GetCompetitors(request.Engine, request.Search, targetUrl);
            return Ok(competitors);
        }
        catch(InvalidOperationException ex)
        {
            return StatusCode(500, ex.Message);
        }
        catch (StaleElementReferenceException)
        {
            return StatusCode(500, "Unable to scrape DuckDuckGo after 3 retries. Please try again later.");
        }
        catch (WebDriverTimeoutException)
        {
            return StatusCode(500, "DuckDuckGo scraper timed out. Please try again later. ");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}