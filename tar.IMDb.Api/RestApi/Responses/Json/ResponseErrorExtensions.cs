namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class ResponseErrorExtensions {
    public string Code { get; set; }
    public string ErrorType { get; set; }
    public bool IsRetryable { get; set; }
    public ResponseErrorLocation[] Locations { get; set; }
  }
}