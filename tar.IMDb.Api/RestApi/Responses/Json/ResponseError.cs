namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class ResponseError {
    public ResponseErrorExtensions Extensions { get; set; }
    public ResponseErrorLocation[] Locations { get; set; }
    public string Message { get; set; }
  }
}