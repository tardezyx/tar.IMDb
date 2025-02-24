using HtmlAgilityPack;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tar.IMDb.Api.Extensions;
using tar.IMDb.Api.Parser;
using tar.IMDb.Api.RestApi.Responses.Html;

namespace tar.IMDb.Api.RestApi {
  internal class RestApiTitleViaHtml : IRestApiTitleViaHtml {
    private readonly string _language;
    private readonly RestClient _restClient;

    public RestApiTitleViaHtml(string language) {
      _language = language;
      _restClient = new RestClient("https://www.imdb.com");

      _restClient.AddDefaultHeaders(
        new Dictionary<string, string>() {
          {"Accept", "text/html,application/xhtml+xml,application/xml"},
          {"User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/114.0"},
        }
      );
    }

    public async Task<MainPage> GetMainPageAsync(string imdbID) {
      HtmlDocument htmlDocument = await RequestHtmlDocumentAsync(
        $"title/{imdbID}",
        true
      );

      return MainPageParser.Parse(htmlDocument);
    }

    public async Task<ReferencePage> GetReferencePageAsync(string imdbID) {
      HtmlDocument htmlDocument = await RequestHtmlDocumentAsync(
        $"title/{imdbID}/reference"
      );

      return ReferencePageParser.Parse(htmlDocument);
    }

    private async Task<HtmlDocument> RequestHtmlDocumentAsync(string requestPath, bool withCulture = false) {
      RestRequest request = new RestRequest(requestPath, Method.Get);
      if (withCulture) {
        request.AddHeader("Accept-Language", _language);
        request.AddHeader("x-imdb-client-name", "imdb-web-next-localized");
        request.AddHeader("x-imdb-user-language", _language);
      }

      RestResponse response = await _restClient.ExecuteAsync(request);

      if (response is null || !response.IsSuccessful || response.Content is null) {
        return null;
      }

      HtmlDocument result = new HtmlDocument();
      result.LoadHtml(response.Content);

      if (result != null) {
        List<HtmlNode> nodes = new List<HtmlNode>();
        nodes.AddRange(result.DocumentNode.Descendants("iframe").EmptyIfNull());
        nodes.AddRange(result.DocumentNode.Descendants("path").EmptyIfNull());
        nodes.AddRange(result.DocumentNode.Descendants("script[not(@type=\"application/json\"]").EmptyIfNull());
        nodes.AddRange(result.DocumentNode.Descendants("style").EmptyIfNull());
        nodes.AddRange(result.DocumentNode.Descendants("svg").EmptyIfNull());
        nodes.AddRange(result.DocumentNode.SelectNodes("//span[@class=\"titlereference-change-view-link\"]").EmptyIfNull());
        nodes.AddRange(result.DocumentNode.SelectNodes("//div[@class=\"ipl-rating-star ipl-rating-interactive__star--empty \"]").EmptyIfNull());
        nodes.AddRange(result.DocumentNode.SelectNodes("//div[@class=\"ipl-rating-interactive  ipl-rating-interactive--no-rating\"]").EmptyIfNull());
        nodes.AddRange(result.DocumentNode.SelectNodes("//div[@class=\"ipl-rating-star ipl-rating-interactive__star\"]").EmptyIfNull());
        nodes.AddRange(result.DocumentNode.SelectNodes("//div[@class=\"ipl-rating-selector\"]").EmptyIfNull());
        nodes.AddRange(result.DocumentNode.SelectNodes("//section[@class=\"titlereference-section-media\"]").EmptyIfNull());
        nodes.AddRange(result.DocumentNode.SelectNodes("//a[@class=\"ipl-header__edit-link\"]").EmptyIfNull());
        nodes.AddRange(result.DocumentNode.SelectNodes("//a").Where(x => x.InnerText.Trim() == "See more &raquo;").EmptyIfNull());
        nodes.AddRange(result.DocumentNode.SelectNodes("//em[@class=\"nobr\"]").EmptyIfNull());
        nodes.AddRange(result.DocumentNode.SelectNodes("//li").Where(x => x.InnerText.Trim() == string.Empty).EmptyIfNull());

        for (int i = nodes.Count(); i > 0; i--) {
          nodes.ElementAt(i - 1).Remove();
        }
      }

      return result;
    }
  }
}