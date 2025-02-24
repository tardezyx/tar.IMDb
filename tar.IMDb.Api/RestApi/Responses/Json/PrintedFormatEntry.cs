using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class PrintedFormatEntry {
    public IEnumerable<StringText> Attributes { get; set; }
    public string PrintedFormat { get; set; }
  }
}