using System;
using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Html {
  public class ReferencePage {
    public string AspectRatio { get; set; }
    public string Awards { get; set; }
    public IEnumerable<BoxOfficeEntry> BoxOffice { get; set; }
    public IEnumerable<Certification> Certifications { get; set; }
    public string Color { get; set; }
    public IEnumerable<string> Countries { get; set; }
    public Crew Crew { get; set; }
    public IEnumerable<string> Genres { get; set; }
    public string Id { get; set; }
    public string ImageUrl { get; set; }
    public IEnumerable<string> Keywords { get; set; }
    public IEnumerable<string> Languages { get; set; }
    public string LocalizedTitle { get; set; }
    public int? NumberOfSeasons { get; set; }
    public IEnumerable<ExternalLink> OfficialSites { get; set; }
    public string OriginalTitle { get; set; }
    public string Outline { get; set; }
    public IEnumerable<Company> ProductionCompanies { get; set; }
    public int? Rank { get; set; }
    public decimal? Rating { get; set; }
    public int? RatingVotes { get; set; }
    public string ReleaseCountry { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public TimeSpan? Runtime { get; set; }
    public IEnumerable<string> SoundMix { get; set; }
    public string Summary { get; set; }
    public string Tagline { get; set; }
    public string Type { get; set; }
    public string Url { get; set; }
    public int? YearFrom { get; set; }
    public int? YearTo { get; set; }
  }
}