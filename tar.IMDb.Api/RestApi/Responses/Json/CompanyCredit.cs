using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class CompanyCredit {
    public IEnumerable<StringText> Attributes { get; set; }
    public Company Company { get; set; }
    public IEnumerable<IdAndText> Countries { get; set; }
    public DisplayableProperty DisplayableProperty { get; set; }
    public Years YearsInvolved { get; set; }
  }
}