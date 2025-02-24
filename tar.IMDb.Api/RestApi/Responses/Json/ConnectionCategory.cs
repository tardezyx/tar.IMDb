namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class ConnectionCategory {
    public IdAndText Category { get; set; }
    public Entries<Connection> Connections { get; set; }
  }
}