namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class ParentsGuideEntry {
    public IdAndText Category { get; set; }
    public string Id { get; set; }
    public bool? IsSpoiler { get; set; }
    public Body Text { get; set; }
  }
}