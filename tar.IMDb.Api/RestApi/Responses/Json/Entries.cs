using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class Entries<T> where T : class {
    public IEnumerable<Entry<T>> Edges { get; set; }
    public PageInfo PageInfo { get; set; }
    public int? Total { get; set; }
  }
}