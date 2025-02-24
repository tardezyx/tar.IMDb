using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class CountryEntries {
    public IEnumerable<IdAndText> Countries { get; set; }
  }
}