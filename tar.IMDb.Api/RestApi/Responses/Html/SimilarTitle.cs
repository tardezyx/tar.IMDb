using System;
using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Html {
  public class SimilarTitle {
    public string Certificate { get; set; }
    public IEnumerable<string> Genres { get; set; }
    public string ID { get; set; }
    public string ImageUrl { get; set; }
    public string LocalizedTitle { get; set; }
    public string OriginalTitle { get; set; }
    public decimal? Rating { get; set; }
    public int? RatingVotes { get; set; }
    public TimeSpan? Runtime { get; set; }
    public string Type { get; set; }
    public string Url { get; set; }
    public int? YearFrom { get; set; }
    public int? YearTo { get; set; }
  }
}