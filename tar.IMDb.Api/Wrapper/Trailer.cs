using System;

namespace tar.IMDb.Api.Wrapper {
  public class Trailer {
    public string Description { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public TimeSpan? Runtime { get; set; }
    public string Thumbnail { get; set; }
    public string Url { get; set; }
  }
}