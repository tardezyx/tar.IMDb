namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class ExternalLinkCategory {
    public IdAndText Category { get; set; }
    public Entries<ExternalLink> ExternalLinks { get; set; }
  }
}