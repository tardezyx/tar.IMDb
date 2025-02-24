namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class Entry<T> where T : class {
    public string Cursor { get; set; }
    public T Node { get; set; }
    public int? Position { get; set; }
  }
}