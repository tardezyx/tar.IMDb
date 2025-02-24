using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class Cameras {
    public IEnumerable<CameraEntry> Items { get; set; }
    public int? Total { get; set; }
  }
}