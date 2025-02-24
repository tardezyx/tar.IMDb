using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class Company {
    public StringText CompanyText { get; set; }
    public IEnumerable<IdAndText> CompanyTypes { get; set; }
    public string Id { get; set; }
  }
}