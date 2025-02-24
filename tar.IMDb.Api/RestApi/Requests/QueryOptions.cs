using tar.IMDb.Api.Enums;

namespace tar.IMDb.Api.RestApi.Requests {
  internal class QueryOptions {
    public AwardsFilter? AwardsFilter { get; set; }
    public string Category { get; set; }
    public string Country { get; set; }
    public bool IsAutoTranslationEnabled { get; set; }
    public string Language { get; set; }
    public int? MaxNumberOfResults { get; set; }
    public PlotType? PlotType { get; set; }
    public string PreviousEndCursor { get; set; }
    public int? Season { get; set; }
    public bool ShowOriginalTitleText { get; set; }
    public string TitleId { get; set; }
    public string Type { get; set; }

    internal QueryOptions(
      AwardsFilter? awardsFilter = null,
      string category = null,
      string country = null,
      bool isAutoTranslationEnabled = false,
      string language = null,
      int? maxNumberOfResults = null,
      PlotType? plotType = null,
      string previousEndCursor = null,
      int? season = null,
      bool showOriginalTitleText = true,
      string titleId = null,
      string type = null
    ) {
      AwardsFilter = awardsFilter;
      Category = category;
      Country = country;
      IsAutoTranslationEnabled = isAutoTranslationEnabled;
      Language = language;
      MaxNumberOfResults = maxNumberOfResults;
      PlotType = plotType;
      PreviousEndCursor = previousEndCursor;
      Season = season;
      ShowOriginalTitleText = showOriginalTitleText;
      TitleId = titleId;
      Type = type;
    }
  }
}