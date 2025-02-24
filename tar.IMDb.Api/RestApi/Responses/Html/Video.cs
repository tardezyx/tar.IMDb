using System;

namespace tar.IMDb.Api.RestApi.Responses.Html {
  public class Video {
    public string Id { get; set; }
    public string ImageUrl { get; set; }
    public string Name { get; set; }
    public TimeSpan? Runtime { get; set; }
    public string Type { get; set; }
    public string Url { get; set; }
  }
}