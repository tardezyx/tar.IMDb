using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class ParentsGuideCategory {
    public IdAndText Category { get; set; }
    public Entries<ParentsGuideEntry> GuideItems { get; set; }
    public Severity Severity { get; set; }
    public IEnumerable<Severity> SeverityBreakdown { get; set; }
    public int? TotalSeverityVotes { get; set; }
  }
}