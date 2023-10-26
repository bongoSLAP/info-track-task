using InfoScrape.Core.Interfaces;
using InfoScrape.Handlers;
using InfoScrape.Services.Clients;
using InfoScrape.Services.Factories;
using InfoScrape.Services.Scrapers;

const string policyName = "CorsPolicy";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddScoped<IScraper, GoogleScraper>();
builder.Services.AddTransient<IScraper, DuckDuckGoScraper>();
builder.Services.AddTransient<IScraperFactory, ScraperFactory>();
builder.Services.AddTransient<ISearchSeleniumClient, SearchSeleniumClient>();
builder.Services.AddTransient<ISearchHttpClient, SearchHttpClient>();
builder.Services.AddTransient<IRankSearchHandler, RankSearchHandler>();
builder.Services.AddTransient<ICompetitorSearchHandler, CompetitorSearchHandler>();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: policyName, policyBuilder =>
    {
        policyBuilder.WithOrigins("https://localhost:44465")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(policyName);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();