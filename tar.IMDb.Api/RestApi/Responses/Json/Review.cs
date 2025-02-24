namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class Review {
    public Author Author { get; set; }
    public int? AuthorRating { get; set; }
    public Helpfulness Helpfulness { get; set; }
    public string Id { get; set; }
    public bool? Spoiler { get; set; }
    public string SubmissionDate { get; set; }
    public StringOriginalText Summary { get; set; }
    public ReviewText Text { get; set; }
  }
}