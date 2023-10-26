using InfoScrape.Core.Interfaces;
using InfoScrape.Core.Models;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;

namespace InfoScrape.Controllers;

public class RankSearchController : BaseController
{
    private readonly IRankSearchHandler _handler;
    
    public RankSearchController(IRankSearchHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("Search/Rank")]
    public IActionResult List([FromBody] SearchRequest? request)
    {
        if (!IsRequestValid(request))
            return BadRequest("Request object is invalid.");
        
        try
        {
            var targetUrl = request!.TargetUrl ?? "https://www.infotrack.co.uk";
            IEnumerable<int> ranks = _handler.GetRanks(request.Engine, request.Search, targetUrl);
            return Ok(ranks);
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