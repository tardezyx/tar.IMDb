using System.Collections.Generic;

namespace tar.IMDb.Api.Wrapper {
  public class Plot {
    public string Outline { get; set; }
    public string OutlineLocalized { get; set; }
    public List<string> Summaries { get; set; } = new List<string>();
    public string Synopsis { get; set; }
  }
}