namespace tar.IMDb.Api.RestApi.Requests {
  internal class RequestBody {
    public string OperationName { get; set; }
    public string Query { get; set; }
    public RequestBodyVariables Variables { get; set; }
  }
}