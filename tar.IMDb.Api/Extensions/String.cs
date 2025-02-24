using System;
using System.Linq;

namespace tar.IMDb.Api.Extensions {
  public static partial class Extensions {
    public static int GetNthIndex(this string source, char charToFind, int occurrences) {
      if (occurrences <= 0 || source.IsNullOrEmpty() || !source.Contains(charToFind)) {
        return -1;
      }

      int result = -1;
      int occurrence = 0;
      for (int i = 0; i < source.Length; i++) {
        if (source[i] == charToFind) {
          occurrence++;
          if (occurrence == occurrences) {
            result = i;
            break;
          }
        }
      }

      return result;
    }

    public static int GetOccurrences(this string source, char charToFind) {
      if (source.IsNullOrEmpty() || !source.Contains(charToFind)) {
        return 0;
      }

      int result = 0;
      foreach (char c in source) {
        if (c == charToFind) {
          result++;
        }
      }

      return result;
    }
    public static int GetOccurrences(this string source, string stringToFind) {
      if (source.IsNullOrEmpty() || !source.Contains(stringToFind)) {
        return 0;
      }

      int result = 0;
      string searchString = source;

      while (true) {
        int index = searchString.IndexOf(stringToFind);

        if (index == -1) {
          break;
        }

        result++;

        if (searchString.Substring(index).Length <= stringToFind.Length) {
          break;
        }

        searchString = searchString.Substring(index + stringToFind.Length);
      }

      return result;
    }

    public static string GetSubstringAfterLastOccurrence(this string source, char charToFind) {
      if (source.IsNullOrEmpty() || !source.Contains(charToFind)) {
        return string.Empty;
      }

      return source.GetSubstringAfterOccurrence(charToFind, source.GetOccurrences(charToFind));
    }

    public static string GetSubstringAfterOccurrence(this string source, char charToFind, int occurrences) {
      if (occurrences <= 0 || source.IsNullOrEmpty() || !source.Contains(charToFind)) {
        return string.Empty;
      }

      int indexFrom = source.GetNthIndex(charToFind, occurrences) + 1;
      return source.Substring(indexFrom);
    }

    public static string GetSubstringAfterString(this string source, string stringToFind) {
      if (source.IsNullOrEmpty() || !source.Contains(stringToFind)) {
        return string.Empty;
      }

      int indexFrom = source.IndexOf(stringToFind) + stringToFind.Length;
      return source.Substring(indexFrom);
    }

    public static string GetSubstringBeforeLastOccurrence(this string source, char charToFind) {
      if (source.IsNullOrEmpty() || !source.Contains(charToFind)) {
        return string.Empty;
      }

      return source.GetSubstringBeforeOccurrence(charToFind, source.GetOccurrences(charToFind));
    }

    public static string GetSubstringBeforeOccurrence(this string source, char charToFind, int occurrences) {
      if (source.IsNullOrEmpty() || !source.Contains(charToFind)) {
        return string.Empty;
      }

      int indexTo = source.GetNthIndex(charToFind, occurrences);
      return source.Substring(0, indexTo);
    }

    public static string GetSubstringBeforeString(this string source, string stringToFind) {
      if (source.IsNullOrEmpty() || !source.Contains(stringToFind)) {
        return string.Empty;
      }

      int indexTo = source.IndexOf(stringToFind);
      return source.Substring(0, indexTo);
    }

    public static string GetSubstringBetweenChars(
      this string source,
      char charToFindBegin,
      char charToFindEnd
    ) {
      if (source.IsNullOrEmpty() || charToFindBegin == charToFindEnd) {
        return string.Empty;
      }

      int indexBegin = source.GetNthIndex(charToFindBegin, source.GetOccurrences(charToFindBegin)) + 1;
      int indexEnd = source.GetNthIndex(charToFindEnd, source.GetOccurrences(charToFindEnd));

      if (indexBegin < 1 || indexEnd < 2) {
        return string.Empty;
      }

      return source.Substring(indexBegin, indexEnd - indexBegin);
    }

    public static string GetSubstringBetweenCharsWithOccurrences(
      this string source,
      char charToFindBegin,
      char charToFindEnd,
      int occurrencesBegin,
      int occurrencesEnd
    ) {
      if (occurrencesBegin <= 0 || occurrencesEnd <= 0 || source.IsNullOrEmpty()) {
        return string.Empty;
      }

      int indexBegin = source.GetNthIndex(charToFindBegin, occurrencesBegin) + 1;
      int indexEnd = source.GetNthIndex(charToFindEnd, occurrencesEnd);

      if (indexBegin < 1 || indexEnd < 2) {
        return string.Empty;
      }

      return source.Substring(indexBegin, indexEnd - indexBegin);
    }

    public static string GetSubstringBetweenStrings(
      this string source,
      string stringBefore,
      string stringAfter
    ) {
      return source
        .GetSubstringAfterString(stringBefore)
        .GetSubstringBeforeString(stringAfter);
    }

    public static string GetWithMergedWhitespace(this string source) {
      if (source.IsNullOrEmpty()) {
        return source;
      }

      return string.Join(
        " ",
        source
        .Split(' ')
        .Where(s => !string.IsNullOrWhiteSpace(s))
      );
    }

    public static string GetWithReplacedSubstrings(
      this string source,
      string substringStartsWith,
      string substringEndsWith,
      string replacement
    ) {
      if (source.IsNullOrEmpty()) {
        return string.Empty;
      }

      string result = source;

      while (true) {
        string prefix = string.Empty;
        string suffix = string.Empty;

        int indexFrom = result.IndexOf(substringStartsWith);
        int indexTo   = -1;

        if (indexFrom >= 0) {
          prefix = result.Substring(0, indexFrom);

          string rest = result.Substring(indexFrom + substringStartsWith.Length);
          indexTo = rest.IndexOf(substringEndsWith);
          if (indexTo >= 0) {
            suffix = rest.Substring(indexTo + substringEndsWith.Length);
          }
        }

        if (indexFrom >= 0 && indexTo >= 0) {
          result = prefix + replacement + suffix;
        } else {
          break;
        }
      }

      return result;
    }

    public static bool HasText(this string source) {
      return !source.IsNullOrEmpty();
    }

    public static bool IsNullOrEmpty(this string source) {
      return string.IsNullOrWhiteSpace(source);
    }

    public static string TrimEnd(this string source, string endString) {
      if (source.IsNullOrEmpty() || !source.EndsWith(endString)) {
        return source;
      }

      return source.Remove(source.LastIndexOf(endString));
    }
  }
}