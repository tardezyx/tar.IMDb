using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using tar.IMDb.Api.Extensions;
using tar.IMDb.Api.RestApi.Responses.Html;

namespace tar.IMDb.Api.Parser {
  internal static class ReferencePageParser {
    internal static ReferencePage Parse(HtmlDocument htmlDocument) {
      HtmlNode rootNode = htmlDocument?
        .DocumentNode
        .SelectSingleNode("//section[@class=\"article listo content-advisories-index\"]");

      HtmlNode constNode = rootNode?
        .SelectSingleNode(".//div[@data-caller-name=\"title\"]");

      string id = constNode?.Attributes["data-tconst"].Value;

      if (id is null) {
        return null;
      }

      HtmlNode boxOfficeNode = rootNode?
        .SelectSingleNode("//section[@class=\"titlereference-section-box-office\"]");

      #region --- header --------------------------------------------------------------------------
      HtmlNode headerNode = rootNode?
        .SelectSingleNode("//div[@class=\"titlereference-header\"]");

      string imageUrl = GeneralParser.GetImageUrl(
        headerNode?
        .SelectSingleNode("//img[@class=\"titlereference-primary-image\"]")?
        .Attributes["src"]?
        .Value
      );

      string localizedTitle = headerNode?
        .SelectSingleNode("//h3[@itemprop=\"name\"]")?
        .ChildNodes[0]?
        .InnerText
        .Trim();

      string originalTitle = headerNode?
        .SelectSingleNode("//h3[@itemprop=\"name\"]")?
        .NextSibling?
        .InnerText
        .Trim();

      if (originalTitle.IsNullOrEmpty()) {
        originalTitle = localizedTitle;
      }

      string yearsText = headerNode?
        .SelectSingleNode("//span[@class=\"titlereference-title-year\"]")?
        .ChildNodes[1]?
        .InnerText
        .Trim();

      int? yearFrom = null;
      int? yearTo = null;
      if (yearsText.HasText()) {
        if (yearsText.Contains('-')) {
          yearFrom = GeneralParser.GetInt(yearsText.GetSubstringBeforeOccurrence('-', 1));
          yearTo = GeneralParser.GetInt(yearsText.GetSubstringAfterOccurrence('-', 1));
        } else {
          yearFrom = GeneralParser.GetInt(yearsText);
        }
      }

      List<HtmlNode> headerList = headerNode?
        .Descendants("ul")
        .FirstOrDefault()?
        .Descendants("li")
        .ToList();

      List<string> genres = new List<string>();
      string releaseText = null;
      string runtimeText = null;
      string type = null;

      if (headerList != null) {
        HtmlNode headerGenres = null;
        HtmlNode headerReleaseText = null;
        HtmlNode headerRuntimeText = null;
        HtmlNode headerType = null;

        if (headerList.Count == 2) {
          headerGenres = headerList[0];
          headerType = headerList[1];
        } else if (headerList.Count == 3) {
          HtmlNode headerList0a = headerList[0]
            .Descendants("a")
            .FirstOrDefault();

          HtmlNode headerList1a = headerList[1]
            .Descendants("a")
            .FirstOrDefault();

          if (headerList0a != null && headerList0a.Attributes["href"].Value.Contains("/genre/")) {
            headerGenres = headerList[0];
            headerReleaseText = headerList[1];
          } else if (headerList1a != null && headerList1a.Attributes["href"].Value.Contains("/genre/")) {
            headerGenres = headerList[1];
          }

          headerType = headerList[2];
        } else if (headerList.Count == 4) {
          headerRuntimeText = headerList[1];
          headerGenres = headerList[2];
          headerType = headerList[3];
        } else if (headerList.Count > 4) {
          headerRuntimeText = headerList[1];
          headerGenres = headerList[2];
          headerReleaseText = headerList[3];
          headerType = headerList[4];
        }

        genres.AddRange(headerGenres?
          .InnerText
          .Replace("\n", string.Empty)
          .Split(',')
          .Select(x => x.Trim())
          .ToList()
          .EmptyIfNull());

        releaseText = headerReleaseText?
          .InnerText
          .Trim();

        runtimeText = headerRuntimeText?
          .InnerText
          .Trim();

        type = headerType?
          .InnerText
          .Trim();
      }

      decimal? rating = GeneralParser.GetDecimal(
        headerNode?
        .SelectSingleNode("//span[@class=\"ipl-rating-star__rating\"]")?
        .InnerText
        .Trim()
      );

      int? ratingVotes = GeneralParser.GetInt(
        headerNode?
        .SelectSingleNode("//span[@class=\"ipl-rating-star__total-votes\"]")?
        .InnerText
        .Trim()
        .GetSubstringBetweenChars('(', ')')
      );

      TimeSpan? runtime = null;
      if (runtimeText.HasText()) {
        if (runtimeText.Contains("h") && runtimeText.Contains("m")) {
          runtime = GeneralParser.GetTimeSpan(
            runtimeText.GetSubstringBeforeOccurrence('h', 1),
            runtimeText.GetSubstringBetweenChars('h', 'm'),
            null
          );
        } else if (runtimeText.Contains("h")) {
          runtime = GeneralParser.GetTimeSpan(
            runtimeText.GetSubstringBeforeOccurrence('h', 1),
            null,
            null
          );
        } else if (runtimeText.Contains("m")) {
          runtime = GeneralParser.GetTimeSpan(
            null,
            runtimeText.GetSubstringBeforeOccurrence('m', 1),
            null
          );
        }
      }

      string releaseCountry = null;
      DateTime? releaseDate = null;
      if (releaseText != null) {
        releaseCountry = releaseText.GetSubstringBetweenChars('(', ')');
        releaseDate = GeneralParser.GetDateTime(
          releaseText.GetSubstringBeforeOccurrence('(', 1)
        );
      }

      string rankText = headerNode?
        .Descendants("a")
        .FirstOrDefault(
          x => x.Attributes["href"] != null
            && x.Attributes["href"]
                .Value
                .StartsWith("/chart/top")
        )?
        .InnerText
        .Trim()
        .GetSubstringAfterOccurrence('#', 1);

      int? rank = GeneralParser.GetInt(rankText);
      #endregion
      #region --- overview ------------------------------------------------------------------------
      HtmlNode overviewNode = rootNode?
        .SelectSingleNode("//section[@class=\"titlereference-section-overview\"]");

      string seasonsText = overviewNode?
        .Descendants("div")
        .FirstOrDefault()?
        .Descendants("a")
        .FirstOrDefault(
          x => x.Attributes["href"] != null
            && x.Attributes["href"].Value.Contains("season=")
        )?
        .Attributes["href"]
        .Value
        .GetSubstringAfterString("season=");

      int? numberOfSeasons = null;
      if (seasonsText.HasText()) {
        numberOfSeasons = int.Parse(seasonsText);
      }

      string outline = overviewNode?
        .Descendants("div")
        .FirstOrDefault(x => x.ChildNodes.Count == 1)?
        .InnerText
        .Trim();


      IEnumerable<HtmlNode> overviewList = overviewNode?
        .Descendants("div")
        .Where(
          x => x.Attributes["class"]?
                .Value == "titlereference-overview-section"
        );

      string awards = null;

      foreach (HtmlNode node in overviewList.EmptyIfNull()) {
        if (node.InnerText.Trim().StartsWith("Awards:")) {
          awards = node
            .Descendants("ul")
            .FirstOrDefault()?
            .InnerText
            .Replace("\n", string.Empty)
            .Trim();
        }
      }

      HtmlNode creditsNode = rootNode?
        .SelectSingleNode("//section[@class=\"titlereference-section-credits\"]");
      #endregion
      #region --- storyline -----------------------------------------------------------------------
      HtmlNode storylineNode = rootNode?
        .SelectSingleNode("//section[@class=\"titlereference-section-storyline\"]");

      List<Certification> certification = new List<Certification>();
      List<string> keywords = new List<string>();

      IEnumerable<HtmlNode> storylineCertificationList = storylineNode?
        .Descendants("td")
        .FirstOrDefault(x => x.InnerText.Trim() == "Certification")?
        .ParentNode
        .Descendants("a");

      foreach (HtmlNode a in storylineCertificationList.EmptyIfNull()) {
        string certificationCountry = a
          .Attributes["href"]
          .Value
          .GetSubstringBetweenStrings("=", "%3");

        string certificationNote = a
          .NextSibling?
          .InnerText
          .Replace("\n", string.Empty)
          .Trim();

        string certificationValue = a
          .InnerText
          .GetSubstringAfterOccurrence(':', 1);

        certification.Add(
          new Certification() {
            Country = certificationCountry,
            Note = certificationNote,
            Value = certificationValue
          }
        );
      }

      IEnumerable<HtmlNode> storylineGenreList = storylineNode?
        .Descendants("td")?
        .FirstOrDefault(x => x.InnerText.Trim() == "Genres")
        .ParentNode?
        .Descendants("a");

      foreach (HtmlNode a in storylineGenreList.EmptyIfNull()) {
        string genre = a
          .Attributes["href"]
          .Value
          .GetSubstringAfterString("/genre/");

        if (genres.FirstOrDefault(x => x == genre) == null) {
          genres.Add(genre);
        }
      }

      IEnumerable<HtmlNode> storylineKeywordList = storylineNode?
        .Descendants("td")?
        .FirstOrDefault(x => x.InnerText.Trim() == "Plot Keywords")
        .ParentNode?
        .Descendants("a");

      foreach (HtmlNode a in storylineKeywordList.EmptyIfNull()) {
        string keyword = a
          .Attributes["href"]
          .Value
          .GetSubstringAfterString("/keyword/");

        if (keyword.HasText()) {
          keywords.Add(keyword);
        }
      }

      string summary = storylineNode?
        .Descendants("td")?
        .FirstOrDefault(x => x.InnerText.Trim() == "Plot Summary")?
        .ParentNode?
        .Descendants("p")?
        .FirstOrDefault()?
        .InnerText
        .Replace("\n", string.Empty)
        .Trim();

      string tagline = storylineNode?
        .Descendants("td")?
        .FirstOrDefault(x => x.InnerText.Trim() == "Taglines")?
        .ParentNode?
        .Descendants("td")?
        .FirstOrDefault(x => x.Attributes["class"] == null)?
        .InnerText
        .Replace("\n", string.Empty)
        .Trim();
      #endregion
      #region --- additional details --------------------------------------------------------------
      HtmlNode detailsNode = rootNode?
        .SelectSingleNode("//section[@class=\"titlereference-section-additional-details\"]");

      string aspectRatio = detailsNode?
        .Descendants("td")
        .FirstOrDefault(x => x.InnerText.Trim() == "Aspect Ratio")?
        .ParentNode
        .Descendants("li")
        .FirstOrDefault()?
        .InnerText
        .Replace("\n", string.Empty)
        .Trim();

      string color = detailsNode?
        .Descendants("td")
        .FirstOrDefault(x => x.InnerText.Trim() == "Color")?
        .ParentNode
        .Descendants("a")
        .FirstOrDefault()?
        .InnerText
        .Trim();

      if (runtime is null) {
        string detailsRuntimeText = detailsNode?
          .Descendants("td")
          .FirstOrDefault(x => x.InnerText.Trim() == "Runtime")?
          .ParentNode
          .Descendants("li")
          .FirstOrDefault()?
          .InnerText
          .GetSubstringBeforeOccurrence('m', 1);

        if (detailsRuntimeText.HasText()) {
          runtime = GeneralParser.GetTimeSpan(null, detailsRuntimeText, null);
        }
      }
      #endregion

      return new ReferencePage() {
        AspectRatio = aspectRatio,
        Awards = awards,
        BoxOffice = ParseBoxOffice(boxOfficeNode),
        Certifications = certification,
        Color = color,
        Countries = ParseCountries(detailsNode),
        Crew = ParseCrew(overviewList, creditsNode),
        Genres = genres,
        Id = id,
        ImageUrl = imageUrl,
        Keywords = keywords,
        Languages = ParseLanguages(detailsNode),
        LocalizedTitle = localizedTitle,
        NumberOfSeasons = numberOfSeasons,
        OfficialSites = ParseOfficialSites(detailsNode),
        OriginalTitle = originalTitle,
        Outline = outline,
        ProductionCompanies = ParseProductionCompanies(creditsNode?.NextSibling?.NextSibling),
        Rank = rank,
        Rating = rating,
        RatingVotes = ratingVotes,
        ReleaseCountry = releaseCountry,
        ReleaseDate = releaseDate,
        Runtime = runtime,
        SoundMix = ParseSoundMix(detailsNode),
        Summary = summary,
        Tagline = tagline,
        Type = type,
        Url = $"https://www.imdb.com/title/{id}",
        YearFrom = yearFrom,
        YearTo = yearTo
      };
    }

    private static Person ParseActor(HtmlNode node) {
      string imageUrl = GeneralParser.GetImageUrl(
        node
        .Descendants("img")
        .FirstOrDefault()?
        .Attributes["loadlate"]?
        .Value
      );

      HtmlNode actor = node
        .Descendants("td")
        .FirstOrDefault(
          x => x.Attributes["itemprop"]?
                .Value == "actor"
        );

      string id = null;
      string name = null;
      string notesText = null;

      if (actor != null) {
        id = actor
          .Descendants("a")
          .FirstOrDefault()?
          .Attributes["href"]?
          .Value
          .GetSubstringBetweenStrings("/name/", "/?");

        name = actor
          .Descendants("span")
          .FirstOrDefault()?
          .InnerText
          .Trim();

        notesText = GeneralParser.GetPlainText(
          node
          .Descendants("td")
          .FirstOrDefault(
            x => x.Attributes["class"]?
                  .Value == "character"
          )?
          .Descendants("div")
          .FirstOrDefault()?
          .InnerText
          .Replace("\n", string.Empty)
          .Trim()
          .GetWithMergedWhitespace());
      } else {
        actor = node
          .Descendants("td")
          .FirstOrDefault(x => x.Attributes["class"] is null);

        id = actor?
          .Descendants("a")
          .FirstOrDefault()?
          .Attributes["href"]?
          .Value
          .GetSubstringBetweenStrings("/name/", "/?");

        name = actor?
          .Descendants("a")
          .FirstOrDefault()?
          .InnerText
          .Trim();

        HtmlNode tdCharacter = node
          .Descendants("td")
          .FirstOrDefault(
            x => x.Attributes["class"]?
                  .Value == "character"
          );

        notesText = GeneralParser.GetPlainText(
          tdCharacter?
          .InnerText
          .Replace("\n", string.Empty)
          .Trim()
          .GetWithMergedWhitespace()
        );

        HtmlNode episodes = tdCharacter?
          .Descendants("a")
          .FirstOrDefault(
            x => x.Attributes["class"]?
                  .Value == "toggle-episodes"
          );

        if (episodes != null) {
          string episodesText = episodes
            .InnerText
            .Replace("\n", string.Empty)
            .Trim()
            .GetWithMergedWhitespace();

          notesText = notesText?
            .Replace(episodesText, $"/ {episodesText}");
        }
      }

      List<string> notes = new List<string>();
      if (notesText.HasText()) {
        if (notesText.Contains("/")) {
          notes = notesText
            .Split('/')
            .Select(x => x.Trim())
            .ToList();
        } else {
          notes.Add(notesText.Trim());
        }
      }

      return new Person() {
        Id = id,
        ImageUrl = imageUrl,
        Name = name,
        Notes = notes.Count > 0 ? string.Join(" | ", notes) : null,
        Url = $"https://www.imdb.com/name/{id}"
      };
    }

    private static List<BoxOfficeEntry> ParseBoxOffice(HtmlNode node) {
      List<BoxOfficeEntry> result = new List<BoxOfficeEntry>();

      IEnumerable<HtmlNode> boxOfficeEntries = node?
        .Descendants("tr");

      foreach (HtmlNode tr in boxOfficeEntries.EmptyIfNull()) {
        string description = tr
          .Descendants("td")
          .FirstOrDefault(x => x.Attributes["class"] != null)?
          .InnerText
          .Trim();

        string boText = tr
          .Descendants("td")
          .FirstOrDefault(x => x.Attributes["class"] == null)?
          .InnerText
          .Replace(", ", "; ")
          .Trim();

        string amountText = null;
        string notes = null;

        if (boText.HasText()) {
          if (boText.Contains(";")) {
            amountText = boText.GetSubstringBeforeOccurrence(';', 1);
            notes = boText.GetSubstringAfterOccurrence(';', 1).Trim();
          } else if (boText.Contains(" (")) {
            amountText = boText.GetSubstringBeforeOccurrence('(', 1).Trim();
            notes = "(" + boText.GetSubstringAfterOccurrence('(', 1).Trim();
          } else {
            amountText = boText.Trim();
          }
        }

        string currency = null;
        long? amount = null;

        if (amountText.HasText()) {
          int amountIndex = amountText.IndexOfAny("0123456789".ToCharArray());

          if (amountIndex > 0) {
            currency = amountText.Substring(0, amountIndex);
          }

          if (amountIndex >= 0) {
            amount = GeneralParser.GetLong(amountText.Substring(amountIndex).Replace(",", string.Empty));
          }
        }

        if (notes.HasText() && !notes.Contains("(")) {
          notes = DateTime.Parse(notes, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
        }

        result.Add(
          new BoxOfficeEntry() {
            Amount = amount,
            Description = description,
            Currency = currency,
            Notes = notes
          }
        );
      }

      return result.Count > 0
        ? result
        : null;
    }

    private static Company ParseCompany(HtmlNode node) {
      HtmlNode a = node
        .Descendants("a")
        .FirstOrDefault();

      string id = a?
        .Attributes["href"]?
        .Value
        .GetSubstringBetweenStrings("/company/", "/");

      string name = a?
        .InnerText
        .Trim();

      return new Company() {
        Id = id,
        Name = name
      };
    }

    private static List<string> ParseCountries(HtmlNode node) {
      List<string> result = new List<string>();

      IEnumerable<HtmlNode> detailsCountries = node?
        .Descendants("td")
        .FirstOrDefault(x => x.InnerText.Trim() == "Country")?
        .ParentNode
        .Descendants("a");

      foreach (HtmlNode a in detailsCountries.EmptyIfNull()) {
        string country = a
          .Attributes["href"]?
          .Value
          .GetSubstringAfterString("/country/");

        if (country.HasText()) {
          result.Add(country);
        }
      }

      return result.Count > 0
        ? result
        : null;
    }

    private static Crew ParseCrew(IEnumerable<HtmlNode> overviewList, HtmlNode creditsNode) {
      List<Person> Actors = new List<Person>();
      List<Person> Directors = new List<Person>();
      List<Person> Writers = new List<Person>();

      foreach (HtmlNode node in overviewList.EmptyIfNull()) {
        IEnumerable<HtmlNode> linkList = node.Descendants("a");

        foreach (HtmlNode personNode in linkList.EmptyIfNull()) {
          Person person = ParsePerson(personNode);

          if (person is null) {
            continue;
          }

          if (node.InnerText.Trim().StartsWith("Stars:")) {
            Actors.Add(person);
          }

          if (node.InnerText.Trim().StartsWith("Director:")) {
            Directors.Add(person);
          }

          if (node.InnerText.Trim().StartsWith("Writer:")) {
            Writers.Add(person);
          }
        }
      }

      IEnumerable<HtmlNode> tables = creditsNode?.Descendants("table");

      foreach (HtmlNode table in tables.EmptyIfNull()) {
        string category = table
          .PreviousSibling?
          .PreviousSibling?
          .InnerText
          .Replace("Series", string.Empty)
          .Replace("\n", string.Empty)
          .Trim()
          .ToLower();

        if (category.IsNullOrEmpty()) {
          continue;
        }

        if (
          category == "cast" ||
          category == "cast summary" ||
          category.Contains("awaiting verification") ||
          category.Contains("in credits order") ||
          category.Contains("verified as complete")
        ) {
          IEnumerable<HtmlNode> entries = table
            .Descendants("tr")
            .Where(
              x => (
                x.Attributes["class"]?.Value == "odd" ||
                x.Attributes["class"]?.Value == "even"
              ) &&
              x.ChildNodes.Count(c => c.Name == "td") > 1
            );

          foreach (HtmlNode tr in entries.EmptyIfNull()) {
            Person actor = ParseActor(tr);

            if (actor is null) {
              continue;
            }

            if (Actors.FirstOrDefault(a => a.Id == actor.Id) is Person existingActor) {
              if (existingActor.ImageUrl.IsNullOrEmpty()) {
                existingActor.ImageUrl = actor.ImageUrl;
              }

              if (existingActor.Notes.IsNullOrEmpty()) {
                existingActor.Notes = actor.Notes;
              }
            } else {
              Actors.Add(actor);
            }
          }
        } else {
          IEnumerable<HtmlNode> entries = table
            .Descendants("tr")
            .Where(
              x => x.ChildNodes.Count(c => c.Name == "td") > 1
            );

          foreach (HtmlNode tr in entries.EmptyIfNull()) {
            Person crewMember = ParseCrewMember(tr);

            if (crewMember is null) {
              continue;
            }

            switch (category) {
              case "directed by":
                if (Directors.FirstOrDefault(a => a.Id == crewMember.Id) is Person existingDirector) {
                  if (existingDirector.ImageUrl.IsNullOrEmpty()) {
                    existingDirector.ImageUrl = crewMember.ImageUrl;
                  }

                  if (existingDirector.Notes.IsNullOrEmpty()) {
                    existingDirector.Notes = crewMember.Notes;
                  }
                } else {
                  Directors.Add(crewMember);
                }
                break;
              
              case "written by":
                if (Writers.FirstOrDefault(a => a.Id == crewMember.Id) is Person existingWriter) {
                  if (existingWriter.ImageUrl.IsNullOrEmpty()) {
                    existingWriter.ImageUrl = crewMember.ImageUrl;
                  }

                  if (existingWriter.Notes.IsNullOrEmpty()) {
                    existingWriter.Notes = crewMember.Notes;
                  }
                } else {
                  Writers.Add(crewMember);
                }
                break;
            }
          }
        }
      }

      if (Actors.Count is 0 && Directors.Count is 0 && Writers.Count is 0) {
        return null;
      }

      return new Crew() {
        Actors = Actors,
        Directors = Directors,
        Writers = Writers,
      };
    }

    private static Person ParseCrewMember(HtmlNode node) {
      HtmlNode a = node
        .Descendants("td")
        .FirstOrDefault(
          x => x.Attributes["class"]?
                .Value == "name"
        )?
        .Descendants("a")
        .FirstOrDefault();

      string id = a?
        .Attributes["href"]?
        .Value
        .GetSubstringBetweenStrings("/name/", "/?");

      string name = a?
        .InnerText
        .Trim();

      IEnumerable<HtmlNode> list = node
        .Descendants("td")
        .Where(
          x => (
            x.Attributes["class"] is null ||
            x.Attributes["class"].Value == "credit"
          ) &&
          x.InnerText != "..." &&
          x.InnerText.Trim().HasText()
        );

      List<string> notes = new List<string>();

      foreach (HtmlNode td in list.EmptyIfNull()) {
        if (td.InnerText.Contains("/")) {
          notes = td
            .InnerText
            .Split('/')
            .Select(
              x => x
                .Replace("\n", string.Empty)
                .GetWithMergedWhitespace()
                .TrimEnd('&')
                .TrimEnd(" and")
                .Trim()
            )
            .ToList();
        } else {
          notes.Add(td
            .InnerText
            .Replace("\n", string.Empty)
            .GetWithMergedWhitespace()
            .TrimEnd('&')
            .TrimEnd(" and")
            .Trim());
        }
      }

      return new Person() {
        Id = id,
        Name = name,
        Notes = notes.Count > 0 ? string.Join(" | ", notes) : null,
        Url = $"https://www.imdb.com/name/{id}"
      };
    }

    private static List<string> ParseLanguages(HtmlNode node) {
      List<string> result = new List<string>();

      IEnumerable<HtmlNode> detailsLanguages = node?
        .Descendants("td")
        .FirstOrDefault(x => x.InnerText.Trim() == "Language")?
        .ParentNode
        .Descendants("a");

      foreach (HtmlNode a in detailsLanguages.EmptyIfNull()) {
        string language = a
          .Attributes["href"]?
          .Value
          .GetSubstringAfterString("/language/");

        if (language.HasText()) {
          result.Add(language);
        }
      }

      return result.Count > 0
        ? result
        : null;
    }

    private static List<ExternalLink> ParseOfficialSites(HtmlNode node) {
      List<ExternalLink> result = new List<ExternalLink>();

      IEnumerable<HtmlNode> detailsOfficialSites = node?
        .Descendants("td")
        .FirstOrDefault(x => x.InnerText.Trim() == "Official Sites")?
        .ParentNode
        .Descendants("a");

      foreach (HtmlNode a in detailsOfficialSites.EmptyIfNull()) {
        string officialSiteURL = a
          .Attributes["href"]?
          .Value;

        if (officialSiteURL.HasText()) {
          result.Add(
            new ExternalLink() {
              Category = "official",
              Label = a
                .InnerText
                .Replace("\n", string.Empty)
                .Trim(),
              URL = officialSiteURL
            }
          );
        }
      }

      return result.Count > 0
        ? result
        : null;
    }

    private static Person ParsePerson(HtmlNode node) {
      string id = node?
        .Attributes["href"]?
        .Value
        .GetSubstringAfterString("/name/");

      if (id.IsNullOrEmpty()) {
        return null;
      }

      return new Person() {
        Id = id,
        Name = node.InnerText.Trim(),
        Url = $"https://www.imdb.com/name/{id}"
      };
    }

    private static List<Company> ParseProductionCompanies(HtmlNode node) {
      List<Company> result = new List<Company>();

      IEnumerable<HtmlNode> companyList = node?
        .Descendants("ul");

      foreach (HtmlNode ul in companyList.EmptyIfNull()) {
        string category = ul
          .PreviousSibling?
          .PreviousSibling?
          .InnerText
          .Replace("\n", string.Empty)
          .Trim()
          .ToLower();

        if (category.IsNullOrEmpty() || category != "production companies") {
          continue;
        }

        foreach (HtmlNode li in ul.Descendants("li").EmptyIfNull()) {
          result.Add(ParseCompany(li));
        }
      }

      return result.Count > 0
        ? result
        : null;
    }

    private static List<string> ParseSoundMix(HtmlNode node) {
      List<string> result = new List<string>();

      IEnumerable<HtmlNode> detailsSoundMixes = node?
        .Descendants("td")
        .FirstOrDefault(x => x.InnerText.Trim() == "Sound Mix")?
        .ParentNode
        .Descendants("a");

      foreach (HtmlNode a in detailsSoundMixes.EmptyIfNull()) {
        result.Add(a.InnerText.Trim());
      }

      return result.Count > 0
        ? result
        : null;
    }
  }
}