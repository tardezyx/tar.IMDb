namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class ImageTypeEntry {
    public ImageType ImageType { get; set; }
    public Entries<Image> Images { get; set; }
  }
}