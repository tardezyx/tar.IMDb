using System.Collections.Generic;

namespace tar.IMDb.Api.Wrapper {
  public class Season {
    public decimal? AverageRating { get; set; }
    public decimal? AverageRatingPercentage { get; set; }
    public int Id { get; set; }
    public List<Title> Episodes { get; set; } = new List<Title>();
    public string Url { get; set; }
    public int? YearFrom { get; set; }
    public int? YearTo { get; set; }
  }
}