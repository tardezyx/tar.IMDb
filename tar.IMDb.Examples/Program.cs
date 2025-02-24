using System.Diagnostics;
using tar.IMDb.Api;
using tar.IMDb.Api.Enums;
using tar.IMDb.Examples;

// === set title ==================================================================================
//string imdbTitle = "tt0068646"; // the godfather
//string imdbTitle = "tt0080684"; // star wars 5
//string imdbTitle = "tt0096697"; // the simpsons
//string imdbTitle = "tt0701055"; // the simpsons s05e12
//string imdbTitle = "tt3416742"; // what we do in the shadows
string imdbTitle = "tt4093826"; // twin peaks s03
//string imdbTitle = "tt4706152"; // twin peaks s03e16
//string imdbTitle = "tt6718170"; // super mario bros
//string imdbTitle = "tt13345606"; // evil dead

// === examples with separate classes =============================================================
// get title infos via wrapper
TestWrapper testWrapper = new(imdbTitle);
await testWrapper.RunTestAsync();

// get title infos via json requests
var testJson = new TestJson();
var testJsonResult = await testJson.TestTitleViaJson(imdbTitle);

// === direct calls ===============================================================================
IMDbRestClient imdbRestClient = new();

// get title infos via filtered json requests
var aggregateRatingsBreakdownWhenGermany = await imdbRestClient.TitleViaJson.GetAggregateRatingsBreakdownAsync(imdbTitle, country: "DE");
var awardNominationsWhenWin = await imdbRestClient.TitleViaJson.GetAwardNominationsAsync(imdbTitle, AwardsFilter.WinsOnly);
var companyCreditsWhenProduction = await imdbRestClient.TitleViaJson.GetCompanyCreditsAsync(imdbTitle, category: "production");
var connectionsWhenReferencedIn = await imdbRestClient.TitleViaJson.GetConnectionsAsync(imdbTitle, category: "referenced_in");
var creditsWhenCast = await imdbRestClient.TitleViaJson.GetCreditsAsync(imdbTitle, category: "cast");
var episodesWhenSeason16 = await imdbRestClient.TitleViaJson.GetEpisodesAsync(imdbTitle, season: 16);
var externalLinksWhenPhoto = await imdbRestClient.TitleViaJson.GetExternalLinksAsync(imdbTitle, category: "photo");
var goofsWhenRevealingMistake = await imdbRestClient.TitleViaJson.GetGoofsAsync(imdbTitle, category: "revealing_mistake");
var imagesWhenPoster = await imdbRestClient.TitleViaJson.GetImagesAsync(imdbTitle, type: "poster");
var parentsGuideWhenProfanity = await imdbRestClient.TitleViaJson.GetParentsGuideAsync(imdbTitle, category: "PROFANITY");
var plotsWhenSummary = await imdbRestClient.TitleViaJson.GetPlotsAsync(imdbTitle, plotType: PlotType.Summary);
var triviaWhenUncategorized = await imdbRestClient.TitleViaJson.GetTriviaAsync(imdbTitle, category: "uncategorized");
var videosMax5 = await imdbRestClient.TitleViaJson.GetVideosAsync(imdbTitle, maxNumberOfResults: 5);

// get title infos via html pages
var mainPage = await imdbRestClient.TitleViaHtml.GetMainPageAsync(imdbTitle);
var referencePage = await imdbRestClient.TitleViaHtml.GetReferencePageAsync(imdbTitle);

// get search results
var searchResults = await imdbRestClient.TitleSearch.GetSearchResultsAsync("5 zimm");

Debugger.Break();