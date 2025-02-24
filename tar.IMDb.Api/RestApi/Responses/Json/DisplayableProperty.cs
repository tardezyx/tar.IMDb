using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class DisplayableProperty {
    public IEnumerable<Body> QualifiersInMarkdownList { get; set; }
    public Body Value { get; set; }
  }
}