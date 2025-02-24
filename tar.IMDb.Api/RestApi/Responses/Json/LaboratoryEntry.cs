using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class LaboratoryEntry {
    public IEnumerable<StringText> Attributes { get; set; }
    public string Laboratory { get; set; }
  }
}