using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class NegativeFormatEntry {
    public IEnumerable<StringText> Attributes { get; set; }
    public string NegativeFormat { get; set; }
  }
}