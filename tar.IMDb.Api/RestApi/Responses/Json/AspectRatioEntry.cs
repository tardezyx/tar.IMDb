using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class AspectRatioEntry {
    public IEnumerable<StringText> Attributes { get; set; }
    public string AspectRatio { get; set; }
  }
}