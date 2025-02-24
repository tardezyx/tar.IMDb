using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class AspectRatios {
    public IEnumerable<AspectRatioEntry> Items { get; set; }
    public int? Total { get; set; }
  }
}