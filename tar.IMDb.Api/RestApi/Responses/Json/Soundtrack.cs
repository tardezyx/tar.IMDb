using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class Soundtrack {
    public IEnumerable<Body> Comments { get; set; }
    public string Id { get; set; }
    public string Text { get; set; }
  }
}