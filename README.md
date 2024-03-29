# InfoScrape: InfoTrack Tech Test

InfoTrack is a .NET + Angular SPA that scrapes Google and DuckDuckGo search results for SEO analysis.

# Setup
There should be no setup needed. Simply run the .NET application in Rider or VS and it should run npm install (if running for the first time) and serve the frontend automatically.
In the case that this does not happen:
1. Navigate to **info-track-task/InfoScrape/InfoScrape/Web** and run **npm install**
2. While still in the web folder, run **ng serve**

# Architecture

The backend has been separated into 4 projects:
- InfoScrape - Main application project, containing controllers, program.cs and appsettings.
- InfoScrape.Core - Domain layer project, containing models, enums and interfaces.
- InfoScrape.Handlers - Application layer project, contains handlers (business logic).
- InfoScrape.Services - Infrastructure layer project, contains external http and selenium clients, scrapers and scraper factory.

# Scrapers

InfoScrape features two scrapers:
- The Google scraper uses a http client to fetch HTML document and parse out the search result urls.
- The DuckDuckGo scraper uses a selenium client to dynamically load and parse out urls via the Chrome driver (the search results are dynamically loaded with react, so fetching the document using a http client will not work). This scraper consequently takes longer than the Google scraper as it needs to load more results multiple times. This also makes the scraper occasionally brittle, so a retry feature has been implemented.

# Demo

![demo](https://i.imgur.com/lH6SXgw.gif)
