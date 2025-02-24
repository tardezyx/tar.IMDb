using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class ReleaseDate {
    public IEnumerable<StringText> Attributes { get; set; }
    public IdAndText Country { get; set; }
    public int? Day { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
  }
}