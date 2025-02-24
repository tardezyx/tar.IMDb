using System;

namespace tar.IMDb.Api.RestApi.Responses.Html {
  public class Episode {
    public int? EpisodeNumber { get; set; }
    public string Id { get; set; }
    public string ImageUrl { get; set; }
    public string LocalizedTitle { get; set; }
    public string OriginalTitle { get; set; }
    public string Plot { get; set; }
    public decimal? Rating { get; set; }
    public int? RatingVotes { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public int? SeasonNumber { get; set; }
    public string Url { get; set; }
  }
}