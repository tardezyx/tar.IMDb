using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class EventMetadataEvents {
    public IEnumerable<EventMetadataEventsEntry> Edges { get; set; }
    public PageInfo PageInfo { get; set; }
    public int Total { get; set; }
  }
}