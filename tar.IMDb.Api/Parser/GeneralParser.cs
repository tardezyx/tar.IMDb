using HtmlAgilityPack;
using System;
using System.Globalization;
using System.Web;
using tar.IMDb.Api.Extensions;

namespace tar.IMDb.Api.Parser {
  internal static class GeneralParser {
    internal static bool? GetBool(string input) {
      if (bool.TryParse(input, out bool result)) {
        return result;
      }

      return null;
    }

    internal static DateTime? GetDateTime(string input) {
      if (input.IsNullOrEmpty()) {
        return null;
      }

      if (DateTime.TryParse(input, out DateTime result)) {
        return result;
      }

      if (input.Length == 4) {
        return GetDateTimeByDMY("1", "1", input);
      }

      return null;
    }

    internal static DateTime? GetDateTimeByDMY(
      string dayInput,
      string monthInput,
      string yearInput
    ) {
      if (yearInput.IsNullOrEmpty()) {
        return null;
      }

      if (!int.TryParse(dayInput, out int day)) {
        day = 1;
      }

      if (!int.TryParse(monthInput, out int month)) {
        month = 1;
      }

      if (int.TryParse(yearInput, out int year)) {
        return new DateTime(year, month, day);
      }

      return null;
    }

    internal static decimal? GetDecimal(string input) {
      if (input.IsNullOrEmpty()) {
        return null;
      }

      if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result)) {
        return result;
      }

      return null;
    }

    internal static string GetHtmlText(string input) {
      if (input is null) {
        return null;
      }

      return input
        .GetWithReplacedSubstrings("?ref", "\"", "\"")
        .GetWithReplacedSubstrings(" class=\"", "\"", string.Empty)
        .Replace("href=\"/", "href=\"https://www.imdb.com/")
        .Replace("src=\"/", "src=\"https://www.imdb.com/");
    }

    internal static string GetImageUrl(string input) {
      if (input.IsNullOrEmpty()) {
        return null;
      }

      return input
        .GetSubstringBeforeLastOccurrence('.')
        .GetSubstringBeforeLastOccurrence('.')
        + '.'
        + input.GetSubstringAfterLastOccurrence('.');
    }

    internal static int? GetInt(string input) {
      if (input.IsNullOrEmpty()) {
        return null;
      }

      if (int.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out int result)) {
        return result;
      }

      return null;
    }

    internal static long? GetLong(string input) {
      if (input.IsNullOrEmpty()) {
        return null;
      }

      if (long.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out long result)) {
        return result;
      }

      return null;
    }

    internal static string GetPlainText(string input) {
      if (input is null) {
        return null;
      }

      string htmlText = GetHtmlText(input)
        .Replace("<br>", Environment.NewLine)
        .Replace("<br/>", Environment.NewLine)
        .Replace("<li>", "<li>- ")
        .Replace("</li>", Environment.NewLine + "</li>");

      HtmlDocument htmlDocument = new HtmlDocument();
      htmlDocument.LoadHtml(htmlText);

      string result = HttpUtility.HtmlDecode(htmlDocument.DocumentNode.InnerText);

      return result.EndsWith(Environment.NewLine)
        ? result.Substring(0, result.Length - Environment.NewLine.Length)
        : result;
    }

    internal static TimeSpan? GetTimeSpan(
      string hoursAsString,
      string minutesAsString,
      string secondsAsString
    ) {
      int? hours = GetInt(hoursAsString);
      int? minutes = GetInt(minutesAsString);
      int? seconds = GetInt(secondsAsString);

      if (
        hours.HasValue && hours > 0 ||
        minutes.HasValue && minutes > 0 ||
        seconds.HasValue && seconds > 0
      ) {
        return new TimeSpan(
          hours.HasValue ? (int)hours : 0,
          minutes.HasValue ? (int)minutes : 0,
          seconds.HasValue ? (int)seconds : 0
        );
      }

      return null;
    }
  }
}