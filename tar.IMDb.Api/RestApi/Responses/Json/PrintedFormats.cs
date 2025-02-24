using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class PrintedFormats {
    public IEnumerable<PrintedFormatEntry> Items { get; set; }
    public int? Total { get; set; }
  }
}