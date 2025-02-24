using System;
using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Html {
  public class MainPage {
    public int? AwardsNominations { get; set; }
    public int? AwardsWins { get; set; }
    public IEnumerable<BoxOfficeEntry> BoxOffice { get; set; }
    public string Certificate { get; set; }
    public IEnumerable<string> Countries { get; set; }
    public Crew Crew { get; set; }
    public IEnumerable<string> Genres { get; set; }
    public string Id { get; set; }
    public string ImageUrl { get; set; }
    public bool? IsEpisode { get; set; }
    public bool? IsSeries { get; set; }
    public IEnumerable<string> Languages { get; set; }
    public string LocalizedTitle { get; set; }
    public int? NumberOfEpisodes { get; set; }
    public int? NumberOfSeasons { get; set; }
    public string OriginalTitle { get; set; }
    public string Outline { get; set; }
    public int? Rank { get; set; }
    public decimal? Rating { get; set; }
    public int? RatingVotes { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public TimeSpan? Runtime { get; set; }
    public Series Series { get; set; }
    public IEnumerable<SimilarTitle> SimilarTitles { get; set; }
    public string Status { get; set; }
    public Technical Technical { get; set; }
    public string Type { get; set; }
    public string Url { get; set; }
    public UserReview UserReview { get; set; }
    public IEnumerable<Video> Videos { get; set; }
    public int? YearFrom { get; set; }
    public int? YearTo { get; set; }
  }
}