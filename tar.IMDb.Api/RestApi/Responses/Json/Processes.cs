using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class Processes {
    public IEnumerable<ProcessEntry> Items { get; set; }
    public int? Total { get; set; }
  }
}