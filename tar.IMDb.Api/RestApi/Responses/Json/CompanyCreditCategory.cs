namespace tar.IMDb.Api.RestApi.Responses.Json {
  public class CompanyCreditCategory {
    public IdAndText Category { get; set; }
    public Entries<CompanyCredit> CompanyCredits { get; set; }
  }
}