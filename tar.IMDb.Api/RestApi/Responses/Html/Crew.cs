using System.Collections.Generic;

namespace tar.IMDb.Api.RestApi.Responses.Html {
  public class Crew {
    public IEnumerable<Person> Actors { get; set; }
    public IEnumerable<Person> Creators { get; set; }
    public IEnumerable<Person> Directors { get; set; }
    public IEnumerable<Person> Writers { get; set; }
  }
}