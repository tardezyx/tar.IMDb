using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using tar.IMDb.Api.RestApi.Responses.Json;

namespace tar.IMDb.Api.RestApi {
  internal class RestApiTitleSearch : IRestApiTitleSearch {
    private readonly string _language;
    private readonly RestClient _restClient;

    public RestApiTitleSearch(string language) {
      _language = language;
      _restClient = new RestClient("https://v3.sg.media-imdb.com");

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

    public async Task<RestResponse<SearchResults>> GetSearchResultsAsync(string searchTerm) {
      string path = $"suggestion/titles/x/{searchTerm}.json?includeVideos=1";

      RestRequest request = new RestRequest(path, Method.Get);
      request.AddHeader("Accept-Language", _language);

      return await _restClient.ExecuteAsync<SearchResults>(request);
    }
  }
}