using HtmlAgilityPack;
using InfoScrape.Core.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace InfoScrape.Services.Clients;

public class SearchSeleniumClient : ISearchSeleniumClient, IDisposable
{
    private ChromeDriver? _driver;
    private readonly ChromeOptions _options;
    
    public SearchSeleniumClient()
    {
        _options = new ChromeOptions();
        _options.AddArgument("--headless");
    }
    
    private ChromeDriver Driver
    {
        get
        {
            if (_driver == null)
                _driver = new ChromeDriver(_options); // remove the driver options to see it in action!
            
            return _driver;
        }
    }

    public HtmlDocument GetDocument(string url)
    {
        Driver.Navigate().GoToUrl(url);
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
        wait.Until(driver => driver.FindElement(By.Id("react-layout")).Displayed);

        for (var i=0; i<14; i++)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            Thread.Sleep(20);
            IWebElement moreResultsButton = Driver.FindElement(By.Id("more-results"));
            moreResultsButton.Click();
            Thread.Sleep(20);
        }

        var document = new HtmlDocument();
        document.LoadHtml(Driver.PageSource);
        return document;
    }

    public void Dispose()
    {
        _driver?.Quit(); // quits out of headless browser window
    }
}