using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class Runtime {
    public IEnumerable<StringText> Attributes { get; set; }
    public string Id { get; set; }
    public int? Seconds { get; set; }
  }
}