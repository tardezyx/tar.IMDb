using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class FilmLengthEntry {
    public IEnumerable<StringText> Attributes { get; set; }
    public float? FilmLength { get; set; }
  }
}