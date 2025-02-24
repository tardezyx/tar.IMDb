namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class RatingsSummary {
    public float? AggregateRating { get; set; }
    public NotificationText NotificationText { get; set; }
    public TopRanking TopRanking { get; set; }
    public int? VoteCount { get; set; }
  }
}