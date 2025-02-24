namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class SeriesEntry {
    public SeriesEpisodeNumber EpisodeNumber { get; set; }
    public Title NextEpisode { get; set; }
    public Title PreviousEpisode { get; set; }
    public Title Series { get; set; }
  }
}