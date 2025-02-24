using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class GenreEntries {
    public IEnumerable<IdAndText> Genres { get; set; }
  }
}