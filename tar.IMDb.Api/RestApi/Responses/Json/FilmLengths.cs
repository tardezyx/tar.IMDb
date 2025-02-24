using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class FilmLengths {
    public IEnumerable<FilmLengthEntry> Items { get; set; }
    public int? Total { get; set; }
  }
}