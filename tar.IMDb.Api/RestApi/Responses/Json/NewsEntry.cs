using System;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class NewsEntry {
    public Body ArticleTitle { get; set; }
    public string Byline { get; set; }
    public DateTime Date { get; set; }
    public string ExternalUrl { get; set; }
    public string Id { get; set; }
    public Image Image { get; set; }
    public Source Source { get; set; }
    public Body Text { get; set; }
  }
}