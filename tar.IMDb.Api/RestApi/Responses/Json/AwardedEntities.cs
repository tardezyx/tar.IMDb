using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class AwardedEntities {
    public IEnumerable<Name> Names { get; set; }
    public IEnumerable<Title> Titles { get; set; }
    public IEnumerable<SecondaryAwardName> SecondaryAwardNames { get; set; }
    public IEnumerable<SecondaryAwardTitle> SecondaryAwardTitles { get; set; }
  }
}