using System.Threading.Tasks;
using tar.IMDb.Api.RestApi.Responses.Html;

namespace tar.IMDb.Api.RestApi {
  public interface IRestApiTitleViaHtml {
    Task<MainPage> GetMainPageAsync(string imdbID);
    Task<ReferencePage> GetReferencePageAsync(string imdbID);
  }
}