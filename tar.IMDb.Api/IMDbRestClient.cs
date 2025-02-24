using System.Globalization;
using tar.IMDb.Api.RestApi;

namespace tar.IMDb.Api {
  public partial class IMDbRestClient {
    private readonly RestApiTitleSearch _restApiTitleSearch;
    private readonly RestApiTitleViaHtml _restApiTitleViaHtml;
    private readonly RestApiTitleViaJson _restApiTitleViaJson;

    public IRestApiTitleSearch TitleSearch => _restApiTitleSearch;
    public IRestApiTitleViaHtml TitleViaHtml => _restApiTitleViaHtml;
    public IRestApiTitleViaJson TitleViaJson => _restApiTitleViaJson;

    public IMDbRestClient(string cultureName = null) {
      string language = cultureName ?? CultureInfo.CurrentCulture.Name;

      _restApiTitleSearch = new RestApiTitleSearch(language);
      _restApiTitleViaHtml = new RestApiTitleViaHtml(language);
      _restApiTitleViaJson = new RestApiTitleViaJson(language);
    }
  }
}