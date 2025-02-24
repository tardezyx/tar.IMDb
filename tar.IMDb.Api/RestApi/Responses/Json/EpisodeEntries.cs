using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class EpisodeEntries {
    public IEnumerable<Entry<Title>> Edges { get; set; }
    public EpisodeEntries Episodes { get; set; }
    public PageInfo PageInfo { get; set; }
    public IntTotal Seasons { get; set; }
    public int? Total { get; set; }
  }
}