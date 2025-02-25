# tar.IMDb.Api

![](https://img.shields.io/nuget/dt/tar.IMDb.Api) [![](https://img.shields.io/nuget/v/tar.IMDb.Api)](https://www.nuget.org/packages/tar.IMDb.Api)

Full [IMDb](https://www.imdb.com) REST API for IMDb titles.

 - [X] C# .NET Standard v2.0
 - [X] Nuget Package: https://www.nuget.org/packages/tar.IMDb.Api

## Function

This library provides the following functionalities to fetch title information from [IMDb](https://www.imdb.com):

- JSON requests 
  - search for titles
  - particular title information plus additional filters, paging, etc.
  - wrapper which contains essential title information as well as collections and seasons
- HTML requests
  - main page
  - reference page

## Usage

There is also an example subproject to show you the usage.

### JSON Requests

You can directly use the client to search for titles:

```cs
IMDbRestClient imdbRestClient = new();
var searchResults = await imdbRestClient.TitleSearch.GetSearchResultsAsync("dark kni");
```

Additionally, you are able to fetch particular title information directly:

```cs
string titleId = "tt0468569"; // the dark knight

// fetch general title information
var response = await imdbRestClient.TitleViaJson.GetTitleAsync(titleId);
var title = response.Data?.Data?.Title;

// fetch first 50 goof entries
response = await imdbRestClient.TitleViaJson.GetGoofsAsync(titleId);
title.Goofs = response.Data?.Data?.Title?.Goofs;
```

Available JSON requests are:

```cs
var aggregateRatingsBreakdown = await imdbRestClient.TitleViaJson.GetAggregateRatingsBreakdownAsync(titleId),
var akas = await imdbRestClient.TitleViaJson.GetAkasAsync(titleId),
var alexaTopQuestions = await imdbRestClient.TitleViaJson.GetAlexaTopQuestionsAsync(titleId),
var alternateVersions = await imdbRestClient.TitleViaJson.GetAlternateVersionsAsync(titleId),
var awardNominations = await imdbRestClient.TitleViaJson.GetAwardNominationsAsync(titleId),
var certificates = await imdbRestClient.TitleViaJson.GetCertificatesAsync(titleId),
var companyCredits = await imdbRestClient.TitleViaJson.GetCompanyCreditsAsync(titleId),
var connections = await imdbRestClient.TitleViaJson.GetConnectionsAsync(titleId),
var crazyCredits = await imdbRestClient.TitleViaJson.GetCrazyCreditsAsync(titleId),
var credits = await imdbRestClient.TitleViaJson.GetCreditsAsync(titleId),
var episodes = await imdbRestClient.TitleViaJson.GetEpisodesAsync(titleId),
var externalLinks = await imdbRestClient.TitleViaJson.GetExternalLinksAsync(titleId),
var faqs = await imdbRestClient.TitleViaJson.GetFaqsAsync(titleId),
var filmingDates = await imdbRestClient.TitleViaJson.GetFilmingDatesAsync(titleId),
var filmingLocations = await imdbRestClient.TitleViaJson.GetFilmingLocationsAsync(titleId),
var goofCategories = await imdbRestClient.TitleViaJson.GetGoofCategoriesAsync(titleId),
var goofs = await imdbRestClient.TitleViaJson.GetGoofsAsync(titleId),
var images = await imdbRestClient.TitleViaJson.GetImagesAsync(titleId),
var keywords = await imdbRestClient.TitleViaJson.GetKeywordsAsync(titleId),
var moreLikeThisTitles = await imdbRestClient.TitleViaJson.GetMoreLikeThisTitlesAsync(titleId),
var news = await imdbRestClient.TitleViaJson.GetNewsAsync(titleId),
var parentsGuide = await imdbRestClient.TitleViaJson.GetParentsGuideAsync(titleId),
var plots = await imdbRestClient.TitleViaJson.GetPlotsAsync(titleId),
var productionDates = await imdbRestClient.TitleViaJson.GetProductionDatesAsync(titleId),
var quotes = await imdbRestClient.TitleViaJson.GetQuotesAsync(titleId),
var releaseDates = await imdbRestClient.TitleViaJson.GetReleaseDatesAsync(titleId),
var reviews = await imdbRestClient.TitleViaJson.GetReviewsAsync(titleId),
var runtimes = await imdbRestClient.TitleViaJson.GetRuntimesAsync(titleId),
var soundtrack = await imdbRestClient.TitleViaJson.GetSoundtrackAsync(titleId),
var taglines = await imdbRestClient.TitleViaJson.GetTaglinesAsync(titleId),
var title = await imdbRestClient.TitleViaJson.GetTitleAsync(titleId),
var trivia = await imdbRestClient.TitleViaJson.GetTriviaAsync(titleId),
var videos = await imdbRestClient.TitleViaJson.GetVideosAsync(titleId),
```

For most JSON requests you can set parameters to filter or page through the results:

- `titleId`: the id of the title on IMDb
- `maxNumberOfResults`: the maximum number of results (default: 50, on categories/types: 1, max: 250)
- `previousEndCursor`: for paged results (should be equal to the endCursor of the previous request)
- `showOriginalTitleText`: no clue if this has any effect, at all (default: true)

There are also some special parameters:

- `awardsFilter`: for `GetAwardNominationsAsync()`
- `category`: filter for the particular category (get the categories beforehand to get their names)
- `country`: for `GetAggregateRatingsBreakdownAsync()`
- `season`: for `GetEpisodesAsync`
- `plotType`: for `GetPlotsAsync`
- `type`: filter for the particular type (get the types beforehand to get their names)

To receive the categories or types, call these methods (be aware, that the number of corresponding sub entries here is set to 1 by default):

```cs
var companyCreditCategories = await imdbRestClient.TitleViaJson.GetCompanyCreditCategoriesAsync(titleId),
var connectionCategories = await imdbRestClient.TitleViaJson.GetConnectionCategoriesAsync(titleId),
var creditCategories = await imdbRestClient.TitleViaJson.GetCreditCategoriesAsync(titleId),
var externalLinkCategories = await imdbRestClient.TitleViaJson.GetExternalLinkCategoriesAsync(titleId),
var imageTypes = await imdbRestClient.TitleViaJson.GetImageTypesAsync(titleId),
var parentsGuideCategories = await imdbRestClient.TitleViaJson.GetParentsGuideCategoriesAsync(titleId),
var triviaCategories = await imdbRestClient.TitleViaJson.GetTriviaCategoriesAsync(titleId)
```

The returned `Response` is encapsulated within the `RestResponse` class which already contains all request and response information:

- `Content`: the received IMDb message data as JSON string
- `Data`: the received IMDb message data as mapped objects

The title structure is predefined via IMDb and only the request corresponding properties are filled.

### JSON Title Wrapper

Within the title wrapper, multiple JSON requests are triggered automatically in parallel to fastly receive essential title information. A provided update event informs about the progress.

```cs
var fallout = new IMDbTitle("tt12637874");
fallout.OnUpdate += OnUpdate; // optionally register/subscribe to the update event
await fallout.FetchAsync();

// your explicit event handler method where you handle on update events
void OnUpdate(ProgressInfo progressInfo) {
  // ... check and use progressInfo for own purposes ...
}
```

The responses are mapped into a smaller but cleaner structure. Existing collections and seasons are automatically collected, sorted and their average rating is also calculated.

For additional title information, you need to use the other JSON or HTML requests directly.

### HTML requests

Furthermore, you can request HTML pages which contain the localized title text but take longer to be received:

```cs
var theDarkKnightMainPage = await imdbRestClient.TitleViaHtml.GetMainPageAsync("tt0468569");
var theDarkKnightReferencePage = await imdbRestClient.TitleViaHtml.GetReferencePageAsync("tt0468569");
```

The responses are again a bit different and aligned to the corresponding page content.

## Known Issues

Last but not least, there are still some minor issues:

- texts and especially localized texts (and title names) are inconsinstently provided by IMDb
  - beside category names, only titles and the main outline plot could be localized
  - if you need localized information, you either need to use & check `TitleViaJson.GetAkasAsync()` or the HTML requests
- video edges/nodes do not return cursors whereby you are not able to page through
  - not as important as mostly you only need a trailer