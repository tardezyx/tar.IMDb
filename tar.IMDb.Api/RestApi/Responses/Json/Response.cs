using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class Response {
    public ResponseData Data { get; set; }
    public IEnumerable<ResponseError> Errors { get; set; }
    public ResponseExtensions Extensions { get; set; }
  }
}