using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class ProcessEntry {
    public IEnumerable<StringText> Attributes { get; set; }
    public string Process { get; set; }
  }
}