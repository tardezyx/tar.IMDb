using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using tar.IMDb.Api.Extensions;
using tar.IMDb.Api.RestApi.Responses.Html;

namespace tar.IMDb.Api.Parser {
  internal static class MainPageParser {
    internal static MainPage Parse(HtmlDocument htmlDocument) {
      string jsonContent = htmlDocument?
        .DocumentNode
        .SelectSingleNode("//script[@type=\"application/json\"]")?
        .InnerText;

      if (jsonContent is null) {
        return null;
      }

      JsonNode nodeSection = JsonNode
        .Parse(jsonContent)?
        ["props"]?
        ["pageProps"];

      if (nodeSection is null) {
        return null;
      }

      JsonNode nodeRoot = JsonNode.Parse(nodeSection.ToJsonString());
      JsonNode nodeAbove = nodeRoot?["aboveTheFoldData"];
      JsonNode nodeMain = nodeRoot?["mainColumnData"];

      string id = nodeMain?["id"]?.ToString();

      if (id.IsNullOrEmpty()) {
        return null;
      }

      return new MainPage() {
        AwardsNominations = GeneralParser.GetInt(nodeMain?["nominationsExcludeWins"]?["total"]?.ToString()),
        AwardsWins = GeneralParser.GetInt(nodeMain?["wins"]?["total"]?.ToString()),
        BoxOffice = ParseBoxOffice(nodeMain),
        Certificate = nodeAbove?["certificate"]?["rating"]?.ToString(),
        Countries = ParseIds(nodeMain?["countriesOfOrigin"]?["countries"]?.AsArray()),
        Crew = ParseCrew(nodeMain),
        Genres = ParseIds(nodeAbove?["genres"]?["genres"]?.AsArray()),
        Id = id,
        ImageUrl = GeneralParser.GetImageUrl(nodeAbove?["primaryImage"]?["url"]?.ToString()),
        IsEpisode = GeneralParser.GetBool(nodeAbove?["titleType"]?["isEpisode"]?.ToString()),
        IsSeries = GeneralParser.GetBool(nodeAbove?["titleType"]?["isSeries"]?.ToString()),
        Languages = ParseIds(nodeMain?["spokenLanguages"]?["spokenLanguages"]?.AsArray()),
        LocalizedTitle = nodeMain?["titleText"]?["text"]?.ToString(),
        NumberOfEpisodes = GeneralParser.GetInt(nodeMain?["episodes"]?["episodes"]?["total"]?.ToString()),
        NumberOfSeasons = nodeMain?["episodes"]?["seasons"]?.AsArray().Count,
        OriginalTitle = nodeMain?["originalTitleText"]?["text"]?.ToString(),
        Outline = nodeAbove?["plot"]?["plotText"]?["plainText"]?.ToString(),
        Rank = GeneralParser.GetInt(nodeMain?["ratingsSummary"]?["topRanking"]?["rank"]?.ToString()),
        Rating = GeneralParser.GetDecimal(nodeAbove?["ratingsSummary"]?["aggregateRating"]?.ToString()),
        RatingVotes = GeneralParser.GetInt(nodeAbove?["ratingsSummary"]?["voteCount"]?.ToString()),
        ReleaseDate = GeneralParser.GetDateTimeByDMY(
          nodeAbove?["releaseDate"]?["day"]?.ToString(),
          nodeAbove?["releaseDate"]?["month"]?.ToString(),
          nodeAbove?["releaseDate"]?["year"]?.ToString()
        ),
        Runtime = GeneralParser.GetTimeSpan(null, null, nodeAbove?["runtime"]?["seconds"]?.ToString()),
        Series = ParseSeries(nodeAbove?["series"]),
        SimilarTitles = ParseSimilarTitles(nodeMain?["moreLikeThisTitles"]?["edges"]?.AsArray()),
        Status = nodeAbove?["productionStatus"]?["currentProductionStage"]?["id"]?.ToString(),
        Technical = ParseTechnical(nodeMain?["technicalSpecifications"]),
        Type = nodeAbove?["titleType"]?["id"]?.ToString(),
        Url = $"https://www.imdb.com/title/{id}",
        UserReview = ParseUserReview(nodeMain?["featuredReviews"]?["edges"]?.AsArray().FirstOrDefault()),
        Videos = ParseVideos(nodeMain?["videoStrip"]?["edges"]?.AsArray()),
        YearFrom = GeneralParser.GetInt(nodeAbove?["releaseYear"]?["year"]?.ToString()),
        YearTo = GeneralParser.GetInt(nodeAbove?["releaseYear"]?["endYear"]?.ToString())
      };
    }

    private static List<BoxOfficeEntry> ParseBoxOffice(JsonNode nodeMain) {
      List<BoxOfficeEntry> result = new List<BoxOfficeEntry>();

      long? boxOfficeProductionAmount = GeneralParser.GetLong(
        nodeMain?["productionBudget"]?["budget"]?["amount"]?.ToString()
      );

      long? boxOfficeLifetimeAmount = GeneralParser.GetLong(
        nodeMain?["lifetimeGross"]?["total"]?["amount"]?.ToString()
      );

      long? boxOfficeOpeningAmount = GeneralParser.GetLong(
        nodeMain?["openingWeekendGross"]?["gross"]?["total"]?["amount"]?.ToString()
      );

      long? boxOfficeWorldwideAmount = GeneralParser.GetLong(
        nodeMain?["worldwideGross"]?["total"]?["amount"]?.ToString()
      );

      if (boxOfficeProductionAmount.HasValue) {
        result.Add(
          new BoxOfficeEntry() {
            Amount = boxOfficeProductionAmount,
            Currency = nodeMain?["productionBudget"]?["budget"]?["currency"]?.ToString(),
            Description = "Budget"
          }
        );
      }

      if (boxOfficeLifetimeAmount.HasValue) {
        result.Add(
          new BoxOfficeEntry() {
            Amount = boxOfficeLifetimeAmount,
            Currency = nodeMain?["lifetimeGross"]?["total"]?["currency"]?.ToString(),
            Description = "Lifetime"
          }
        );
      }

      if (boxOfficeOpeningAmount.HasValue) {
        result.Add(
          new BoxOfficeEntry() {
            Amount = boxOfficeOpeningAmount,
            Currency = nodeMain?["openingWeekendGross"]?["gross"]?["total"]?["currency"]?.ToString(),
            Description = "Opening Weekend",
            Notes = nodeMain?["openingWeekendGross"]?["weekendEndDate"]?.ToString()
          }
        );
      }

      if (boxOfficeWorldwideAmount.HasValue) {
        result.Add(
          new BoxOfficeEntry() {
            Amount = boxOfficeWorldwideAmount,
            Currency = nodeMain?["worldwideGross"]?["total"]?["currency"]?.ToString(),
            Description = "Worldwide"
          }
        );
      }

      return result.Count > 0
        ? result
        : null;
    }

    private static Crew ParseCrew(JsonNode nodeMain) {
      List<Person> actors = new List<Person>();
      List<Person> creators = new List<Person>();
      List<Person> directors = new List<Person>();
      List<Person> writers = new List<Person>();

      if (nodeMain?["cast"]?["edges"]?.AsArray().Count > 0) {
        JsonArray nodeCast = nodeMain?["cast"]?["edges"]?.AsArray();

        foreach (JsonNode node in nodeCast.EmptyIfNull()) {
          if (ParsePerson(node?["node"]) is Person person) {
            actors.Add(person);
          }
        }
      }

      if (nodeMain?["creators"]?.AsArray().Count > 0) {
        JsonArray nodeCreators = nodeMain?["creators"]?.AsArray()[0]?["credits"]?.AsArray();

        foreach (JsonNode node in nodeCreators.EmptyIfNull()) {
          if (ParsePerson(node) is Person person) {
            creators.Add(person);
          }
        }
      }

      if (nodeMain?["directors"]?.AsArray().Count > 0) {
        JsonArray nodeDirectors = nodeMain?["directors"]?.AsArray()[0]?["credits"]?.AsArray();

        foreach (JsonNode node in nodeDirectors.EmptyIfNull()) {
          if (ParsePerson(node) is Person person) {
            directors.Add(person);
          }
        }
      }

      if (nodeMain?["writers"]?.AsArray().Count > 0) {
        JsonArray nodeWriters = nodeMain?["writers"]?.AsArray()[0]?["credits"]?.AsArray();

        foreach (JsonNode node in nodeWriters.EmptyIfNull()) {
          string writerID = node?["name"]?["id"]?.ToString();
          string writerName = node?["name"]?["nameText"]?["text"]?.ToString();

          if (writerID.HasText()) {
            writers.Add(
              new Person() {
                Id = writerID,
                Name = writerName,
                Url = $"https://www.imdb.com/name/{writerID}"
              }
            );
          }
        }
      }

      if (actors.Count is 0 && creators.Count is 0 && directors.Count is 0 && writers.Count is 0) {
        return null;
      }

      return new Crew() {
        Actors = actors.Count > 0 ? actors : null,
        Creators = creators.Count > 0 ? creators : null,
        Directors = directors.Count > 0 ? directors : null,
        Writers = writers.Count > 0 ? writers : null
      };
    }

    private static List<string> ParseIds(JsonArray nodes) {
      List<string> result = new List<string>();

      foreach (JsonNode node in nodes.EmptyIfNull()) {
        result.Add(node?["id"]?.ToString());
      }

      return result.Count > 0
        ? result
        : null;
    }

    private static Person ParsePerson(JsonNode nodePerson) {
      string id = nodePerson?["name"]?["id"]?.ToString();

      if (id.IsNullOrEmpty()) {
        return null;
      }

      List<string> notes = new List<string>();

      JsonArray characters = nodePerson?["characters"]?.AsArray();
      foreach (JsonNode nodeCharacter in characters.EmptyIfNull()) {
        string note = nodeCharacter?["name"]?.ToString();
        if (note.HasText()) {
          notes.Add(note);
        }
      }

      JsonNode nodeEpisodeCredits = nodePerson?["episodeCredits"];
      int? numberOfEpisodes = GeneralParser.GetInt(nodeEpisodeCredits?["total"]?.ToString());
      if (numberOfEpisodes > 0) {
        int? yearFrom = GeneralParser.GetInt(nodeEpisodeCredits?["yearRange"]?["year"]?.ToString());
        int? yearTo = GeneralParser.GetInt(nodeEpisodeCredits?["yearRange"]?["endYear"]?.ToString());
        notes.Add($"{numberOfEpisodes} episodes, {yearFrom}-{yearTo}");
      }

      return new Person() {
        Id = id,
        ImageUrl = GeneralParser.GetImageUrl(nodePerson["name"]?["primaryImage"]?["url"]?.ToString()),
        Name = nodePerson?["name"]?["nameText"]?["text"]?.ToString(),
        Notes = notes.Count > 0 ? string.Join(" | ", notes) : null,
        Url = $"https://www.imdb.com/name/{id}"
      };
    }

    private static Series ParseSeries(JsonNode nodeSeries) {
      string id = nodeSeries?["series"]?["id"]?.ToString();

      if (id.IsNullOrEmpty()) {
        return null;
      }

      return new Series() {
        EpisodeNumber = GeneralParser.GetInt(nodeSeries?["episodeNumber"]?["episodeNumber"]?.ToString()),
        Id = id,
        LocalizedTitle = nodeSeries?["series"]?["titleText"]?["text"]?.ToString(),
        OriginalTitle = nodeSeries?["series"]?["originalTitleText"]?["text"]?.ToString(),
        SeasonNumber = GeneralParser.GetInt(nodeSeries?["episodeNumber"]?["seasonNumber"]?.ToString()),
        Type = nodeSeries?["series"]?["titleType"]?["id"]?.ToString(),
        Url = $"https://www.imdb.com/title/{id}",
        YearFrom = GeneralParser.GetInt(nodeSeries?["series"]?["releaseYear"]?["year"]?.ToString()),
        YearTo = GeneralParser.GetInt(nodeSeries?["series"]?["releaseYear"]?["endYear"]?.ToString())
      };
    }

    private static List<SimilarTitle> ParseSimilarTitles(JsonArray nodes) {
      List<SimilarTitle> result = new List<SimilarTitle>();

      foreach (JsonNode node in nodes.EmptyIfNull()) {
        string id = node?["node"]?["id"]?.ToString();

        if (id.IsNullOrEmpty()) {
          continue;
        }

        JsonArray nodeSimilarTitlesGenres = node?
          ["node"]?
          ["titleGenres"]?
          ["genres"]?
          .AsArray();

        List<string> similarTitleGenres = new List<string>();
        foreach (JsonNode nodeGenre in nodeSimilarTitlesGenres.EmptyIfNull()) {
          similarTitleGenres.Add(nodeGenre?["genre"]?["id"]?.ToString());
        }

        result.Add(
          new SimilarTitle() {
            Certificate = node?["node"]?["certificate"]?["rating"]?.ToString(),
            Genres = similarTitleGenres.Count > 0 ? similarTitleGenres : null,
            ID = id,
            ImageUrl = GeneralParser.GetImageUrl(node?["node"]?["primaryImage"]?["url"]?.ToString()),
            LocalizedTitle = node?["node"]?["titleText"]?["text"]?.ToString(),
            OriginalTitle = node?["node"]?["originalTitleText"]?["text"]?.ToString(),
            Rating = GeneralParser.GetDecimal(node?["node"]?["ratingsSummary"]?["aggregateRating"]?.ToString()),
            RatingVotes = GeneralParser.GetInt(node?["node"]?["ratingsSummary"]?["voteCount"]?.ToString()),
            Runtime = GeneralParser.GetTimeSpan(null, null, node?["node"]?["runtime"]?["seconds"]?.ToString()),
            Type = node?["node"]?["titleType"]?["id"]?.ToString(),
            Url = $"https://www.imdb.com/title/{id}",
            YearFrom = GeneralParser.GetInt(node?["node"]?["releaseYear"]?["year"]?.ToString()),
            YearTo = GeneralParser.GetInt(node?["node"]?["releaseYear"]?["endYear"]?.ToString())
          }
        );
      }

      return result.Count > 0
        ? result
        : null;
    }

    private static Technical ParseTechnical(JsonNode nodeTechnical) {
      JsonArray aspectRatioNodes = nodeTechnical?
        ["aspectRatios"]?
        ["items"]?
        .AsArray();

      JsonArray colorationNodes = nodeTechnical?
        ["colorations"]?
        ["items"]?
        .AsArray();

      JsonArray soundMixNodes = nodeTechnical?
        ["soundMixes"]?
        ["items"]?
        .AsArray();

      List<string> aspectRatios = new List<string>();
      foreach (JsonNode node in aspectRatioNodes.EmptyIfNull()) {
        aspectRatios.Add(node?["aspectRatio"]?.ToString());
      }

      List<string> colorations = new List<string>();
      foreach (JsonNode node in colorationNodes.EmptyIfNull()) {
        colorations.Add(node?["text"]?.ToString());
      }

      List<string> soundMixes = new List<string>();
      foreach (JsonNode node in soundMixNodes.EmptyIfNull()) {
        soundMixes.Add(node?["text"]?.ToString());
      }

      if (aspectRatios.Count is 0 && colorations.Count is 0 && soundMixes.Count is 0) {
        return null;
      }

      return new Technical() {
        AspectRatios = aspectRatios.Count > 0 ? aspectRatios : null,
        Colorations = colorations.Count > 0 ? colorations : null,
        SoundMixes = soundMixes.Count > 0 ? soundMixes : null
      };
    }

    private static UserReview ParseUserReview(JsonNode nodeReview) {
      string id = nodeReview?["node"]?["id"]?.ToString();

      if (id.IsNullOrEmpty()) {
        return null;
      }

      return new UserReview() {
        Headline = nodeReview?["node"]?["summary"]?["originalText"]?.ToString(),
        Rating = GeneralParser.GetInt(nodeReview?["node"]?["authorRating"]?.ToString()),
        Text = GeneralParser.GetPlainText(nodeReview?["node"]?["text"]?["originalText"]?["plaidHtml"]?.ToString())
      };
    }

    private static List<Video> ParseVideos(JsonArray nodes) {
      List<Video> result = new List<Video>();

      foreach (JsonNode node in nodes.EmptyIfNull()) {
        string id = node?["node"]?["id"]?.ToString();

        if (id.IsNullOrEmpty()) {
          continue;
        }

        result.Add(
          new Video() {
            Id = id,
            ImageUrl = GeneralParser.GetImageUrl(node?["node"]?["thumbnail"]?["url"]?.ToString()),
            Name = node?["node"]?["name"]?["value"]?.ToString().Trim(),
            Runtime = GeneralParser.GetTimeSpan(null, null, node?["node"]?["runtime"]?["value"]?.ToString()),
            Type = node?["node"]?["contentType"]?["displayName"]?["value"]?.ToString(),
            Url = $"https://www.imdb.com/video/{id}"
          }
        );
      }

      return result.Count > 0
        ? result
        : null;
    }
  }
}