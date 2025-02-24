using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class CameraEntry {
    public IEnumerable<StringText> Attributes { get; set; }
    public string Camera { get; set; }
  }
}