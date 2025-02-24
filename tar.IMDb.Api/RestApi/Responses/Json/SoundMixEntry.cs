using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class SoundMixEntry {
    public IEnumerable<StringText> Attributes { get; set; }
    public string Id { get; set; }
    public string Text { get; set; }
  }
}