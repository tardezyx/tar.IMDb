using System.Collections.Generic;

namespace tar.IMDb.Api.Wrapper {
  public class Crew {
    public List<Person> Actors { get; set; } = new List<Person>();
    public List<Person> Directors { get; set; } = new List<Person>();
    public List<Person> Writers { get; set; } = new List<Person>();
  }
}