using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class SoundMixes {
    public IEnumerable<SoundMixEntry> Items { get; set; }
    public int? Total { get; set; }
  }
}