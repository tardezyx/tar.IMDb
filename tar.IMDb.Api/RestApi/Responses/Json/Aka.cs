using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class Aka {
    public IEnumerable<StringText> Attributes { get; set; }
    public IdAndText Country { get; set; }
    public IdAndText Language { get; set; }
    public string Text { get; set; }
  }
}