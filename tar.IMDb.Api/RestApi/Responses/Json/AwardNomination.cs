namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class AwardNomination {
    public Award Award { get; set; }
    public AwardedEntities AwardedEntities { get; set; }
    public string Id { get; set; }
    public bool? IsWinner { get; set; }
  }
}