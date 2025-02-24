using System;

namespace tar.IMDb.Api.Wrapper {
  public class Title {
    public int? EpisodeNumber { get; set; }
    public string Id { get; set; }
    public Image Image { get; set; } = new Image();
    public string OriginalTitle { get; set; }
    public Rating Rating { get; set; } = new Rating();
    public DateTime? ReleaseDate { get; set; }
    public TimeSpan? Runtime { get; set; }
    public int? SeasonNumber { get; set; }
    public string Type { get; set; }
    public string Url { get; set; }
    public int? YearFrom { get; set; }
    public int? YearTo { get; set; }
  }
}