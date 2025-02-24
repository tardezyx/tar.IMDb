using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using tar.IMDb.Api.Extensions;
using tar.IMDb.Api.Parser;
using tar.IMDb.Api.RestApi.Responses.Json;

namespace tar.IMDb.Api.Converters {
  internal class SearchResultsConverter : JsonConverter<SearchResults> {
    private class LocalSuggestion {
      public IEnumerable<LocalSuggestionD> D { get; set; }
      public string Q { get; set; }
      public int V { get; set; }
    }

    private class LocalSuggestionD {
      public LocalSuggestionI I { get; set; }
      public string Id { get; set; }
      public string L { get; set; }
      public string S { get; set; }
      public string Q { get; set; }
      public string Qid { get; set; }
      public int Rank { get; set; }
      public IEnumerable<LocalSuggestionV> V { get; set; }
      public int Vt { get; set; }
      public int Y { get; set; }
      public string Yr { get; set; }
    }

    private class LocalSuggestionI {
      public int Height { get; set; }
      public string ImageUrl { get; set; }
      public int Width { get; set; }
    }

    private class LocalSuggestionV {
      public LocalSuggestionI I { get; set; }
      public string Id { get; set; }
      public string L { get; set; }
      public string S { get; set; }
    }

    public override bool CanConvert(Type objectType) {
      return true;
    }

    public override SearchResults Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
      if (JsonSerializer.Deserialize<LocalSuggestion>(ref reader, options) is LocalSuggestion localSuggestion) {
        SearchResults result = new SearchResults() {
          SearchTerm = localSuggestion.Q,
          V = localSuggestion.V
        };

        List<SearchResult> searchResults = new List<SearchResult>();

        foreach (LocalSuggestionD d in localSuggestion.D.EmptyIfNull()) {
          SearchResult searchResult = new SearchResult() {
            Cast = d.Q.HasText() ? d.S.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries) : null,
            Id = d.Id,
            Image = new SearchResultImage() {
              Height = d.I.Height,
              Url = d.I.ImageUrl,
              Width = d.I.Width
            },
            NumberOfVideos = d.Vt,
            Rank = d.Rank,
            Title = d.L,
            Type = d.Q,
            TypeId = d.Qid,
            Year = d.Y,
            YearRange = d.Yr
          };

          List<SearchResultVideo> searchResultVideos = new List<SearchResultVideo>();

          foreach (LocalSuggestionV v in d.V.EmptyIfNull()) {
            int occurrences = v.S.GetOccurrences(':');

            string runtimeHours = "0";
            string runtimeMinutes = "0";
            string runtimeSeconds = v.S;;

            if (occurrences == 2) {
              runtimeHours = v.S.GetSubstringBeforeOccurrence(':', 1);
              runtimeMinutes = v.S.GetSubstringBetweenCharsWithOccurrences(':', ':', 1, 2);
              runtimeSeconds = v.S.GetSubstringAfterLastOccurrence(':');
            } else if (occurrences == 1) {
              runtimeMinutes = v.S.GetSubstringBeforeOccurrence(':', 1);
              runtimeSeconds = v.S.GetSubstringAfterOccurrence(':', 1);
            }

            SearchResultVideo searchResultVideo = new SearchResultVideo() {
              Id = v.Id,
              Image = new SearchResultImage() {
                Height = v.I.Height,
                Url = v.I.ImageUrl,
                Width = v.I.Width
              },
              Name = v.L,
              Runtime = GeneralParser.GetTimeSpan(runtimeHours, runtimeMinutes, runtimeSeconds)
            };

            searchResultVideos.Add(searchResultVideo);
          }

          searchResult.Videos = searchResultVideos;
          searchResults.Add(searchResult);
        }

        result.Results = searchResults;
        return result;
      }

      return null;
    }

    public override void Write(Utf8JsonWriter writer, SearchResults value, JsonSerializerOptions options) {
      throw new NotImplementedException();
    }
  }
}