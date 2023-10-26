using InfoScrape.Core.Enums;
using InfoScrape.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace InfoScrape.Controllers;

public class BaseController : Controller
{
    protected BaseController()
    {
        
    }
    
    public bool IsRequestValid(SearchRequest? request)
    {
        var isRequestNull = request == null;
        var isSearchEmpty = string.IsNullOrEmpty(request?.Search);
        var isEngineNull = request?.Engine == null;
        var isEngineInvalid = !isEngineNull && !Enum.IsDefined(typeof(SearchEngine), request!.Engine);
        var isTargetUrlInvalid = request?.TargetUrl != null && string.IsNullOrEmpty(request.TargetUrl);

        return !isRequestNull && !isSearchEmpty && 
               !isEngineNull && !isTargetUrlInvalid && !isEngineInvalid;
    }
}