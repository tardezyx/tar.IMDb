using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class Laboratories {
    public IEnumerable<LaboratoryEntry> Items { get; set; }
    public int? Total { get; set; }
  }
}