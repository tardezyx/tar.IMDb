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
  - wrapper which contains essential title infos
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
string theDarkKnight = "tt0468569";

// base info
var response = await imdbRestClient.TitleViaJson.GetTitleAsync(theDarkKnight);
var theDarkKnightTitle = response.Data?.Data?.Title;

// first 50 goof entries
response = await imdbRestClient.TitleViaJson.GetGoofsAsync(theDarkKnight);
theDarkKnightTitle.Goofs = response.Data?.Data?.Title?.Goofs;
// etc..
```

For most JSON requests, there are additional parameters to filter or page through the results.

The parameters for most methods are:
- `titleId`: the id of the title on IMDb
- `maxNumberOfResults`: the maximum number of results (default: 50, on categories: 1, max: 250)
- `previousEndCursor`: for paged results (should be equal to the endCursor of the previous request)
- `showOriginalTitleText`: no clue if this has any effect, at all (default: true)

Some special parameters are:
- `awardsFilter`: for `GetAwardNominationsAsync()`
- `category`: filter for the particular category (get the categories beforehand to get their names)
- `country`: for `GetAggregateRatingsBreakdownAsync()`
- `season`: for `GetEpisodesAsync`
- `plotType`: for `GetPlotsAsync`
- `type`: filter for the particular type (get the types beforehand to get their names)

The returned `Response` is encapsulated within the RestResponse class which already contains all request and response information.

- `Content`: the received IMDb message data as JSON string
- `Data`: the received IMDb message data as mapped objects

The overall title structure here is predefined via IMDb and filled depending on the actual request.

### Json Wrapper

You can also use the provided wrapper which interally handles fetching and structuring the essential title information:

```cs
var fallout = new IMDbTitle("tt12637874");
fallout.OnUpdate += OnUpdate; // optionally register/subscribe to the update event
await fallout.FetchAsync();

// your explicit event handler method where you handle on update events
void OnUpdate(ProgressInfo progressInfo) {
  // ... check and use progressInfo for own purposes ...
}
```

### Html requests

Furthermore, you can request html pages by which also the localized title should be automatically included:

```cs
var theDarkKnightMainPage = await imdbRestClient.TitleViaHtml.GetMainPageAsync("tt0468569");
var theDarkKnightReferencePage = await imdbRestClient.TitleViaHtml.GetReferencePageAsync("tt0468569");
```

The downside is that this takes much longer than direct JSON requests.

The responses are again a bit different and aligned to the corresponding page content.
