namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class CreditCategory {
    public IdAndText Category { get; set; }
    public Entries<Credit> Credits { get; set; }
  }
}