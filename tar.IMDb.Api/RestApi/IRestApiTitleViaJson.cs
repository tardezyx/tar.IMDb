using RestSharp;
using System.Threading.Tasks;
using tar.IMDb.Api.Enums;
using tar.IMDb.Api.RestApi.Responses.Json;

namespace tar.IMDb.Api.RestApi {
  public interface IRestApiTitleViaJson {
    Task<RestResponse<Response>> GetAggregateRatingsBreakdownAsync(string titleId, string country = null);
    Task<RestResponse<Response>> GetAkasAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetAlexaTopQuestionsAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetAlternateVersionsAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetAwardNominationsAsync(string titleId, AwardsFilter awardsFilter = AwardsFilter.All, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetCertificatesAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetCompanyCreditCategoriesAsync(string titleId, int maxNumberOfResults = 1, bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetCompanyCreditsAsync(string titleId, string category = null, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetConnectionCategoriesAsync(string titleId, int maxNumberOfResults = 1, bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetConnectionsAsync(string titleId, string category = null, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetCrazyCreditsAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetCreditCategoriesAsync(string titleId, int maxNumberOfResults = 1, bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetCreditsAsync(string titleId, string category = null, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetEpisodesAsync(string titleId, int? season = null, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetExternalLinkCategoriesAsync(string titleId, int maxNumberOfResults = 1, bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetExternalLinksAsync(string titleId, string category = null, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetFaqsAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetFilmingDatesAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetFilmingLocationsAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetGoofCategoriesAsync(string titleId, int maxNumberOfResults = 1, bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetGoofsAsync(string titleId, string category = null, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetImagesAsync(string titleId, string type = null, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetImageTypesAsync(string titleId, int maxNumberOfResults = 1, bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetKeywordsAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetMoreLikeThisTitlesAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetNewsAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetParentsGuideAsync(string titleId, string category = null, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetParentsGuideCategoriesAsync(string titleId, int maxNumberOfResults = 1, bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetPlotsAsync(string titleId, PlotType plotType = PlotType.All, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetProductionDatesAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetQuotesAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetReleaseDatesAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetReviewsAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetRuntimesAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetSoundtrackAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetTaglinesAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetTitleAsync(string titleId, bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetTriviaAsync(string titleId, string category = null, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetTriviaCategoriesAsync(string titleId, int maxNumberOfResults = 1, bool showOriginalTitleText = true);
    Task<RestResponse<Response>> GetVideosAsync(string titleId, int maxNumberOfResults = 50, string previousEndCursor = "", bool showOriginalTitleText = true);
  }
}