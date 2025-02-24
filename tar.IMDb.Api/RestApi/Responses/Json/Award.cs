namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class Award {
    public StringText Category { get; set; }
    public IdAndText Event { get; set; }
    public AwardEventEdition EventEdition { get; set; }
    public string Id { get; set; }
    public Body Notes { get; set; }
    public string Text { get; set; }
    public int? WinningRank { get; set; }
  }
}