using RestSharp;
using System.Collections.Concurrent;
using tar.IMDb.Api;
using tar.IMDb.Api.RestApi.Responses.Json;

namespace tar.IMDb.Examples {
  public class TestJson {
    private readonly IMDbRestClient _client = new();

    public async Task<Title?> TestTitleViaJson(string titleId) {
      var tasks = new ConcurrentBag<Task<RestResponse<Response>>> {
        _client.TitleViaJson.GetAggregateRatingsBreakdownAsync(titleId),
        _client.TitleViaJson.GetAkasAsync(titleId),
        _client.TitleViaJson.GetAlexaTopQuestionsAsync(titleId),
        _client.TitleViaJson.GetAlternateVersionsAsync(titleId),
        _client.TitleViaJson.GetAwardNominationsAsync(titleId),
        _client.TitleViaJson.GetCertificatesAsync(titleId),
        _client.TitleViaJson.GetCompanyCreditsAsync(titleId),
        _client.TitleViaJson.GetConnectionsAsync(titleId),
        _client.TitleViaJson.GetCrazyCreditsAsync(titleId),
        _client.TitleViaJson.GetCreditsAsync(titleId),
        _client.TitleViaJson.GetEpisodesAsync(titleId),
        _client.TitleViaJson.GetExternalLinksAsync(titleId),
        _client.TitleViaJson.GetFaqsAsync(titleId),
        _client.TitleViaJson.GetFilmingDatesAsync(titleId),
        _client.TitleViaJson.GetFilmingLocationsAsync(titleId),
        _client.TitleViaJson.GetGoofCategoriesAsync(titleId),
        _client.TitleViaJson.GetGoofsAsync(titleId),
        _client.TitleViaJson.GetImagesAsync(titleId),
        _client.TitleViaJson.GetImageTypesAsync(titleId),
        _client.TitleViaJson.GetKeywordsAsync(titleId),
        _client.TitleViaJson.GetMoreLikeThisTitlesAsync(titleId),
        _client.TitleViaJson.GetNewsAsync(titleId),
        _client.TitleViaJson.GetParentsGuideAsync(titleId),
        _client.TitleViaJson.GetPlotsAsync(titleId),
        _client.TitleViaJson.GetProductionDatesAsync(titleId),
        _client.TitleViaJson.GetQuotesAsync(titleId),
        _client.TitleViaJson.GetReleaseDatesAsync(titleId),
        _client.TitleViaJson.GetReviewsAsync(titleId),
        _client.TitleViaJson.GetRuntimesAsync(titleId),
        _client.TitleViaJson.GetSoundtrackAsync(titleId),
        _client.TitleViaJson.GetTaglinesAsync(titleId),
        _client.TitleViaJson.GetTitleAsync(titleId),
        _client.TitleViaJson.GetTriviaAsync(titleId),
        _client.TitleViaJson.GetVideosAsync(titleId),
        _client.TitleViaJson.GetCompanyCreditCategoriesAsync(titleId),
        _client.TitleViaJson.GetConnectionCategoriesAsync(titleId),
        _client.TitleViaJson.GetCreditCategoriesAsync(titleId),
        _client.TitleViaJson.GetExternalLinkCategoriesAsync(titleId),
        _client.TitleViaJson.GetParentsGuideCategoriesAsync(titleId),
        _client.TitleViaJson.GetTriviaCategoriesAsync(titleId)
      };

      var responses = await Task.WhenAll(
        tasks.AsParallel()
      );

      Title? result = null;

      foreach (
        var response in responses.Where(
          x => x.Data?.Data?.Title is Title title
            && title.Id is not null
        )
      ) {
        result = response?.Data?.Data?.Title;
      }

      if (result is null) {
        return result;
      }

      foreach (var response in responses) {
        if (response.Data?.Data?.Title is Title title && title.Id is null) {
          if (title.AggregateRatingsBreakdown is not null) { result.AggregateRatingsBreakdown = title.AggregateRatingsBreakdown; }
          if (title.Akas is not null) { result.Akas = title.Akas; }
          if (title.AlexaTopQuestions is not null) { result.AlexaTopQuestions = title.AlexaTopQuestions; }
          if (title.AlternateVersions is not null) { result.AlternateVersions = title.AlternateVersions; }
          if (title.AwardNominations is not null) { result.AwardNominations = title.AwardNominations; }
          if (title.CanRate is not null) { result.CanRate = title.CanRate; }
          if (title.Certificate is not null) { result.Certificate = title.Certificate; }
          if (title.Certificates is not null) { result.Certificates = title.Certificates; }
          if (title.CompanyCreditCategories is not null) { result.CompanyCreditCategories = title.CompanyCreditCategories; }
          if (title.CompanyCredits is not null) { result.CompanyCredits = title.CompanyCredits; }
          if (title.ConnectionCategories is not null) { result.ConnectionCategories = title.ConnectionCategories; }
          if (title.Connections is not null) { result.Connections = title.Connections; }
          if (title.CountriesOfOrigin is not null) { result.CountriesOfOrigin = title.CountriesOfOrigin; }
          if (title.CrazyCredits is not null) { result.CrazyCredits = title.CrazyCredits; }
          if (title.CreditCategories is not null) { result.CreditCategories = title.CreditCategories; }
          if (title.Credits is not null) { result.Credits = title.Credits; }
          if (title.Episodes is not null) { result.Episodes = title.Episodes; }
          if (title.ExternalLinkCategories is not null) { result.ExternalLinkCategories = title.ExternalLinkCategories; }
          if (title.ExternalLinks is not null) { result.ExternalLinks = title.ExternalLinks; }
          if (title.Faqs is not null) { result.Faqs = title.Faqs; }
          if (title.FilmingDates is not null) { result.FilmingDates = title.FilmingDates; }
          if (title.FilmingLocations is not null) { result.FilmingLocations = title.FilmingLocations; }
          if (title.Genres is not null) { result.Genres = title.Genres; }
          if (title.GoofCategories is not null) { result.GoofCategories = title.GoofCategories; }
          if (title.Goofs is not null) { result.Goofs = title.Goofs; }
          if (title.Id is not null) { result.Id = title.Id; }
          if (title.ImageTypes is not null) { result.ImageTypes = title.ImageTypes; }
          if (title.Images is not null) { result.Images = title.Images; }
          if (title.IsAdult is not null) { result.IsAdult = title.IsAdult; }
          if (title.Keywords is not null) { result.Keywords = title.Keywords; }
          if (title.LatestTrailer is not null) { result.LatestTrailer = title.LatestTrailer; }
          if (title.LifetimeGrossDomestic is not null) { result.LifetimeGrossDomestic = title.LifetimeGrossDomestic; }
          if (title.LifetimeGrossInternational is not null) { result.LifetimeGrossInternational = title.LifetimeGrossInternational; }
          if (title.LifetimeGrossWorldwide is not null) { result.LifetimeGrossWorldwide = title.LifetimeGrossWorldwide; }
          if (title.Meta is not null) { result.Meta = title.Meta; }
          if (title.Metacritic is not null) { result.Metacritic = title.Metacritic; }
          if (title.MoreLikeThisTitles is not null) { result.MoreLikeThisTitles = title.MoreLikeThisTitles; }
          if (title.News is not null) { result.News = title.News; }
          if (title.OpeningWeekendGrossDomestic is not null) { result.OpeningWeekendGrossDomestic = title.OpeningWeekendGrossDomestic; }
          if (title.OpeningWeekendGrossInternational is not null) { result.OpeningWeekendGrossInternational = title.OpeningWeekendGrossInternational; }
          if (title.OpeningWeekendGrossWorldwide is not null) { result.OpeningWeekendGrossWorldwide = title.OpeningWeekendGrossWorldwide; }
          if (title.OriginalTitleText is not null) { result.OriginalTitleText = title.OriginalTitleText; }
          if (title.ParentsGuide is not null) { result.ParentsGuide = title.ParentsGuide; }
          if (title.ParentsGuideCategories is not null) { result.ParentsGuideCategories = title.ParentsGuideCategories; }
          if (title.Plot is not null) { result.Plot = title.Plot; }
          if (title.Plots is not null) { result.Plots = title.Plots; }
          if (title.PrestigiousAwardSummary is not null) { result.PrestigiousAwardSummary = title.PrestigiousAwardSummary; }
          if (title.PrimaryImage is not null) { result.PrimaryImage = title.PrimaryImage; }
          if (title.ProductionBudget is not null) { result.ProductionBudget = title.ProductionBudget; }
          if (title.ProductionDates is not null) { result.ProductionDates = title.ProductionDates; }
          if (title.ProductionStatus is not null) { result.ProductionStatus = title.ProductionStatus; }
          if (title.Quotes is not null) { result.Quotes = title.Quotes; }
          if (title.RatingsSummary is not null) { result.RatingsSummary = title.RatingsSummary; }
          if (title.ReleaseDate is not null) { result.ReleaseDate = title.ReleaseDate; }
          if (title.ReleaseDates is not null) { result.ReleaseDates = title.ReleaseDates; }
          if (title.ReleaseYear is not null) { result.ReleaseYear = title.ReleaseYear; }
          if (title.Reviews is not null) { result.Reviews = title.Reviews; }
          if (title.Runtime is not null) { result.Runtime = title.Runtime; }
          if (title.Runtimes is not null) { result.Runtimes = title.Runtimes; }
          if (title.Series is not null) { result.Series = title.Series; }
          if (title.Soundtrack is not null) { result.Soundtrack = title.Soundtrack; }
          if (title.SpokenLanguages is not null) { result.SpokenLanguages = title.SpokenLanguages; }
          if (title.StillFrame is not null) { result.StillFrame = title.StillFrame; }
          if (title.Taglines is not null) { result.Taglines = title.Taglines; }
          if (title.TechnicalSpecifications is not null) { result.TechnicalSpecifications = title.TechnicalSpecifications; }
          if (title.TitleText is not null) { result.TitleText = title.TitleText; }
          if (title.TitleType is not null) { result.TitleType = title.TitleType; }
          if (title.Trivia is not null) { result.Trivia = title.Trivia; }
          if (title.TriviaCategories is not null) { result.TriviaCategories = title.TriviaCategories; }
          if (title.VideoStrip is not null) { result.VideoStrip = title.VideoStrip; }
        }
      }

      return result;
    }
  }
}