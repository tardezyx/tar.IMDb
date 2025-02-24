using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class Credit {
    public IEnumerable<StringText> Attributes { get; set; }
    public IdAndText Category { get; set; }
    public IEnumerable<StringName> Characters { get; set; }
    public CreditEpisodes EpisodeCredits { get; set; }
    public IEnumerable<IdAndText> Jobs { get; set; }
    public Name Name { get; set; }
  }
}