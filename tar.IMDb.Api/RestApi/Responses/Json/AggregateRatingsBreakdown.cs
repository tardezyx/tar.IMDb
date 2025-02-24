using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class AggregateRatingsBreakdown {
    public Histogram Histogram { get; set; }
    public IEnumerable<RatingsSummaryByCountry> RatingsSummaryByCountry { get; set; }
  }
}