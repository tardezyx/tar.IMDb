using System.Collections.Generic;
using System.Text.Json.Serialization;
using tar.IMDb.Api.Converters;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  [JsonConverter(typeof(SearchResultsConverter))]
  public class SearchResults {
    public IEnumerable<SearchResult> Results { get; set; }
    public string SearchTerm { get; set; }
    public int? V { get; set; }
  }
}