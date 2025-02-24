using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class NegativeFormats {
    public IEnumerable<NegativeFormatEntry> Items { get; set; }
    public int? Total { get; set; }
  }
}