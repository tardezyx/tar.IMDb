using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class Certificate {
    public IEnumerable<StringText> Attributes { get; set; }
    public IdAndText Country { get; set; }
    public string Id { get; set; }
    public string Rating { get; set; }
    public string RatingReason { get; set; }
    public IdAndText RatingsBody { get; set; }
  }
}