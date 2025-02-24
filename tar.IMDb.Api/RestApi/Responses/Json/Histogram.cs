using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class Histogram {
    public IEnumerable<HistogramValue> HistogramValues { get; set; }
  }
}