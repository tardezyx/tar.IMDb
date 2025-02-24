using RestSharp;
using System.Threading.Tasks;
using tar.IMDb.Api.RestApi.Responses.Json;

namespace tar.IMDb.Api.RestApi {
  public interface IRestApiTitleSearch {
    Task<RestResponse<SearchResults>> GetSearchResultsAsync(string searchTerm);
  }
}