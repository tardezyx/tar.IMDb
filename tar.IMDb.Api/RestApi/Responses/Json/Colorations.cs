using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class Colorations {
    public IEnumerable<ColorationEntry> Items { get; set; }
    public int? Total { get; set; }
  }
}