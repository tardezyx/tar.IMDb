using System.Collections.Generic;

namespace tar.IMDb.Api.Wrapper {
  public class Collection {
    public decimal? AverageRating { get; set; }
    public decimal? AverageRatingPercentage { get; set; }
    public List<Title> Titles { get; set; } = new List<Title>();
  }
}