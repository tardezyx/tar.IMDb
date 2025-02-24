using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class SearchResult {
    public IEnumerable<string> Cast { get; set; }
    public string Id { get; set; }
    public SearchResultImage Image { get; set; }
    public int? NumberOfVideos { get; set; }
    public int? Rank { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public string TypeId { get; set; }
    public IEnumerable<SearchResultVideo> Videos { get; set; }
    public int? Year { get; set; }
    public string YearRange { get; set; }
  }
}