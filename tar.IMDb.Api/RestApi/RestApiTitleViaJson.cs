using RestSharp;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using tar.IMDb.Api.Enums;
using tar.IMDb.Api.RestApi.Requests;
using tar.IMDb.Api.RestApi.Responses.Json;

namespace tar.IMDb.Api.RestApi {
  internal class RestApiTitleViaJson : IRestApiTitleViaJson {
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions() {
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
      NumberHandling = JsonNumberHandling.AllowReadingFromString,
      PropertyNameCaseInsensitive = true,
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    private readonly string _language;
    private readonly RestClient _restClient;

    public RestApiTitleViaJson(string language) {
      _language = language;
      _restClient = new RestClient("https://api.graphql.imdb.com");

      _restClient.AddDefaultHeaders(
        new Dictionary<string, string>() {
          {"Accept", "application/graphql+json, application/json"},
          {"Accept-Language", _language},
          {"Content-Type", "application/json"},
          {"User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/114.0"},
          {"x-imdb-client-name", "imdb-web-next-localized"},
          {"x-imdb-user-language", _language}
        }
      );
    }

    public async Task<RestResponse<Response>> GetAggregateRatingsBreakdownAsync(
      string titleId,
      string country = null
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          country: country,
          language: _language,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleAggregateRatingsBreakdown)
      );
    }

    public async Task<RestResponse<Response>> GetAkasAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleAkas)
      );
    }

    public async Task<RestResponse<Response>> GetAlexaTopQuestionsAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleAlexaTopQuestions)
      );
    }

    public async Task<RestResponse<Response>> GetAlternateVersionsAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleAlternateVersions)
      );
    }

    public async Task<RestResponse<Response>> GetAwardNominationsAsync(
      string titleId,
      AwardsFilter awardsFilter = AwardsFilter.All,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          awardsFilter: awardsFilter,
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleAwardNominations)
      );
    }

    public async Task<RestResponse<Response>> GetCertificatesAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleCertificates)
      );
    }

    public async Task<RestResponse<Response>> GetCompanyCreditCategoriesAsync(
      string titleId,
      int maxNumberOfResults = 1,
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleCompanyCreditCategories)
      );
    }

    public async Task<RestResponse<Response>> GetCompanyCreditsAsync(
      string titleId,
      string category = null,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          category: category,
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleCompanyCredits)
      );
    }

    public async Task<RestResponse<Response>> GetConnectionCategoriesAsync(
      string titleId,
      int maxNumberOfResults = 1,
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleConnectionCategories)
      );
    }

    public async Task<RestResponse<Response>> GetConnectionsAsync(
      string titleId,
      string category = null,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          category: category,
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleConnections)
      );
    }

    public async Task<RestResponse<Response>> GetCrazyCreditsAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleCrazyCredits)
      );
    }

    public async Task<RestResponse<Response>> GetCreditCategoriesAsync(
      string titleId,
      int maxNumberOfResults = 1,
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleCreditCategories)
      );
    }

    public async Task<RestResponse<Response>> GetCreditsAsync(
      string titleId,
      string category = null,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          category: category,
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleCredits)
      );
    }

    public async Task<RestResponse<Response>> GetEpisodesAsync(
      string titleId,
      int? season = null,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          season: season,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleEpisodes)
      );
    }

    public async Task<RestResponse<Response>> GetExternalLinkCategoriesAsync(
      string titleId,
      int maxNumberOfResults = 1,
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleExternalLinkCategories)
      );
    }

    public async Task<RestResponse<Response>> GetExternalLinksAsync(
      string titleId,
      string category = null,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          category: category,
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleExternalLinks)
      );
    }

    public async Task<RestResponse<Response>> GetFaqsAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleFaqs)
      );
    }

    public async Task<RestResponse<Response>> GetFilmingDatesAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleFilmingDates)
      );
    }

    public async Task<RestResponse<Response>> GetFilmingLocationsAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleFilmingLocations)
      );
    }

    public async Task<RestResponse<Response>> GetGoofCategoriesAsync(
      string titleId,
      int maxNumberOfResults = 1,
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleGoofCategories)
      );
    }

    public async Task<RestResponse<Response>> GetGoofsAsync(
      string titleId,
      string category = null,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          category: category,
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleGoofs)
      );
    }

    public async Task<RestResponse<Response>> GetImagesAsync(
      string titleId,
      string type = null,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId,
          type: type
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleImages)
      );
    }

    public async Task<RestResponse<Response>> GetImageTypesAsync(
      string titleId,
      int maxNumberOfResults = 1,
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleImageTypes)
      );
    }

    public async Task<RestResponse<Response>> GetKeywordsAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleKeywords)
      );
    }

    public async Task<RestResponse<Response>> GetMoreLikeThisTitlesAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleMoreLikeThisTitles)
      );
    }

    public async Task<RestResponse<Response>> GetNewsAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleNews)
      );
    }

    public async Task<RestResponse<Response>> GetParentsGuideCategoriesAsync(
      string titleId,
      int maxNumberOfResults = 1,
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleParentsGuideCategories)
      );
    }

    public async Task<RestResponse<Response>> GetParentsGuideAsync(
      string titleId,
      string category = null,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          category: category,
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleParentsGuide)
      );
    }

    public async Task<RestResponse<Response>> GetPlotsAsync(
      string titleId,
      PlotType plotType = PlotType.All,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          plotType: plotType,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitlePlots)
      );
    }

    public async Task<RestResponse<Response>> GetProductionDatesAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleProductionDates)
      );
    }

    public async Task<RestResponse<Response>> GetQuotesAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleQuotes)
      );
    }

    public async Task<RestResponse<Response>> GetReleaseDatesAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleReleaseDates)
      );
    }

    public async Task<RestResponse<Response>> GetReviewsAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleReviews)
      );
    }

    public async Task<RestResponse<Response>> GetRuntimesAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleRuntimes)
      );
    }

    public async Task<RestResponse<Response>> GetSoundtrackAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleSoundtrack)
      );
    }

    public async Task<RestResponse<Response>> GetTaglinesAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleTaglines)
      );
    }

    public async Task<RestResponse<Response>> GetTitleAsync(
      string titleId,
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.Title)
      );
    }

    public async Task<RestResponse<Response>> GetTriviaCategoriesAsync(
      string titleId,
      int maxNumberOfResults = 1,
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleTriviaCategories)
      );
    }

    public async Task<RestResponse<Response>> GetTriviaAsync(
      string titleId,
      string category = null,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          category: category,
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleTrivia)
      );
    }

    public async Task<RestResponse<Response>> GetVideosAsync(
      string titleId,
      int maxNumberOfResults = 50,
      string previousEndCursor = "",
      bool showOriginalTitleText = true
    ) {
      Queryizer query = new Queryizer(
        new QueryOptions(
          language: _language,
          maxNumberOfResults: maxNumberOfResults,
          previousEndCursor: previousEndCursor,
          showOriginalTitleText: showOriginalTitleText,
          titleId: titleId
        )
      );

      return await SendRequestAsync(
        query.GetRequestBody(QueryName.TitleVideos)
      );
    }

    private async Task<RestResponse<Response>> SendRequestAsync(RequestBody body) {
      RestRequest request = new RestRequest("", Method.Post);

      request.AddJsonBody(
        JsonSerializer.Serialize(body, _jsonOptions)
      );

      return await _restClient.ExecuteAsync<Response>(request);
    }
  }
}