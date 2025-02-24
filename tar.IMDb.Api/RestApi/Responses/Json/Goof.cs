namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class Goof {
    public IdAndText Category { get; set; }
    public string Id { get; set; }
    public InterestScore InterestScore { get; set; }
    public bool? IsSpoiler { get; set; }
    public Body Text { get; set; }
  }
}