namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class GoofCategory {
    public IdAndText Category { get; set; }
    public Entries<Goof> Goofs { get; set; }
  }
}