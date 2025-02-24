namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class TriviaCategory {
    public IdAndText Category { get; set; }
    public Entries<TriviaEntry> Trivia { get; set; }
  }
}