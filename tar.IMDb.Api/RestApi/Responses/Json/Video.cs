namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class Video {
    public ContentType ContentType { get; set; }
    public StringValue Description { get; set; }
    public string Id { get; set; }
    public StringValue Name { get; set; }
    public Title PrimaryTitle { get; set; }
    public StringId ProviderType { get; set; }
    public VideoRuntime Runtime { get; set; }
    public Image Thumbnail { get; set; }
  }
}