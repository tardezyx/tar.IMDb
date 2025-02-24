namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class RatingsSummaryByCountry {
    public float? Aggregate { get; set; }
    public string Country { get; set; }
    public StringValue DisplayText { get; set; }
    public int? VoteCount { get; set; }
  }
}