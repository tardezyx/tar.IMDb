using RestSharp;
using System;
using System.Collections.Generic;
using tar.IMDb.Api.Enums;
using tar.IMDb.Api.RestApi.Responses.Json;

namespace tar.IMDb.Api.Wrapper {
  public class ProgressInfo {
    public DateTime? Begin { get; set; }
    public TimeSpan? Duration { get; set; }
    public DateTime? End { get; set; }
    public bool Error { get; set; } = false;
    public WrapperMethod Method { get; set; }
    public List<RestResponse<Response>> Responses { get; set; } = new List<RestResponse<Response>>();
  }
}