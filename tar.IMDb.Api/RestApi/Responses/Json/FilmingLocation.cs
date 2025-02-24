using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class FilmingLocation {
    public IEnumerable<StringText> Attributes { get; set; }
    public string Id { get; set; }
    public InterestScore InterestScore { get; set; }
    public string Location { get; set; }
    public string Text { get; set; }
  }
}