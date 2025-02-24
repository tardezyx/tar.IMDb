using System;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class SearchResultVideo {
    public string Id { get; set; }
    public SearchResultImage Image { get; set; }
    public string Name { get; set; }
    public TimeSpan? Runtime { get; set; }
  }
}