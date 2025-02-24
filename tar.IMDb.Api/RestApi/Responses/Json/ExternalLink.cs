using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class ExternalLink {
    public IdAndText ExternalLinkCategory { get; set; }
    public IEnumerable<IdAndText> ExternalLinkLanguages { get; set; }
    public IdAndText ExternalLinkRegion { get; set; }
    public string Label { get; set; }
    public string Url { get; set; }
  }
}